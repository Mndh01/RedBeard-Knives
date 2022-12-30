using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Data.Specifications;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _genProductRepo;
        private readonly IGenericRepository<ProductCategory> _genCategoryRepo;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        public ProductsController (IGenericRepository<Product> genProductRepo, IPhotoService photoService,
            IGenericRepository<ProductCategory> genCategoryRepo, IMapper mapper) 
        {
            _mapper = mapper;
            _photoService = photoService;
            _genProductRepo = genProductRepo;
            _genCategoryRepo = genCategoryRepo;
        }
        
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {   
            var spec = new ProductsWithTypesSpecification(id);

            var product = await _genProductRepo.GetEntityWithSpec(spec);

            if (product != null)
            {
                return Ok(product);
            }

            return BadRequest("Couldn't find product...");
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams productParams) 
        {
            List<Expression<Func<Product, bool>>> criteria = new List<Expression<Func<Product, bool>>>();

            criteria.Add(p => (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search)));
            criteria.Add(p => (!productParams.CategoryId.HasValue || p.CategoryId == productParams.CategoryId));
            criteria.Add(p => (!productParams.Price.HasValue || p.Price <= productParams.Price));
            criteria.Add(p => (!productParams.InStock || p.InStock > 0));
             
            var spec = new ProductsWithTypesSpecification(productParams, criteria);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _genProductRepo.CountAsync(countSpec);

            var products = await _genProductRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ItemDto>>(products);

            if (products.Count > 0) 
            { 
                return Ok(new Pagination<ItemDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
            }

            return BadRequest("Couldn't find any products...");

            // Using normal repository below
            // var products = await _productRepository.GetItemsAsync(productParams, category, price, inStock, soldItems);

            // if (products != null) {
            //     Response.AddPaginationHeader(new PaginationHeader(products.CurrentPage, 
            //     products.PageSize, products.TotalCount, products.TotalPages));

            //     return Ok(value: products);
            // }
            
            
            // return BadRequest("Couldn't find products with such properties...");
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories() 
        {
            var categories = await _genCategoryRepo.ListAllAsync();

            if (categories != null) return Ok(categories);

            return BadRequest("Couldn't find categories...");
        }
        
        [HttpPost("add-category")]
        public async Task<ActionResult<ProductCategory>> AddCategory(ProductCategory newCategory)
        {
            BaseSpecification<ProductCategory> spec = new BaseSpecification<ProductCategory>(pc => pc.Name == newCategory.Name.ToLower());
            
            var exists = await _genCategoryRepo.GetEntityWithSpec(spec);

            if(exists != null) { return BadRequest("Category already exists..."); }
            
            var category = new ProductCategory { Name = newCategory.Name.ToLower() };

            if (await _genCategoryRepo.AddAsync(category)) return Ok(category);

            return BadRequest("Couldn't add category...");
        }

        [HttpPost("add-product")]
        public async Task<ActionResult<Product>> AddProduct(Product product) 
        {
            var newProduct  = _mapper.Map<Product>(product);
            
            newProduct.Name = newProduct.Name.ToLower();

            BaseSpecification<Product> spec = new BaseSpecification<Product>(p => p.Name == product.Name.ToLower());

            newProduct.Category = await _genCategoryRepo.GetByIdAsync(product.Category.Id);
            
            if (await _genProductRepo.GetEntityWithSpec(spec) != null) return BadRequest("Name already exists for another product..");
                        
            if(await _genProductRepo.AddAsync(newProduct))
                return Ok(newProduct);

            return BadRequest("Failed to add product");               
        }
        
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file, int id)
        {
            
            var product = await _genProductRepo.GetByIdAsync(id);

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo 
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (product.Photos.Count == 0) 
            {
                photo.IsMain = true;
        }

            product.Photos.Add(photo);

            if (await _genProductRepo.SaveAllAsync()) return CreatedAtRoute("GetProduct", new {id = product.Id}, _mapper.Map<PhotoDto>(photo));

            return BadRequest("Failed to add the photo..");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {   
            var dummyProduct = new Product 
            {
                Id = id
            };

            var success = _genProductRepo.Delete(dummyProduct);

            if (success)
                return Ok("Deleted successfully.");
            
            return BadRequest("Failed to remove product..");
        }

        [HttpDelete("delete-photo")]
        public async Task<ActionResult<Product>> DeletePhoto(int ProductId, int PhotoId)
        {
            var product = await _genProductRepo.GetByIdAsync(ProductId);

            var photo = product.Photos.FirstOrDefault(Photo => Photo.Id == PhotoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete the main photo");

            if (photo.PublicId != null) {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }
            
            product.Photos.Remove(photo);

            if (await _genProductRepo.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo..");
        }
        
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductUpdateDto productUpdateDto) 
        {
            productUpdateDto.Name = productUpdateDto.Name.ToLower();

            productUpdateDto.Category.Name = productUpdateDto.Category.Name.ToLower();
            
            var product = await _genProductRepo.GetByIdAsync(productUpdateDto.Id);

            _mapper.Map(productUpdateDto, product);

            _genProductRepo.Update(product);

            if (await _genProductRepo.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update product..");
        }

        [HttpPut("set-main-photo")]
        public async Task<ActionResult> SetMainPhoto(int ProductId, int PhotoId)
        {
            var product = await _genProductRepo.GetByIdAsync(ProductId);

            var photo = product.Photos.FirstOrDefault(photo => photo.Id == PhotoId);

            if (photo.IsMain) return BadRequest("This is already the main photo");

            var currentMain = product.Photos.FirstOrDefault(photo => photo.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _genProductRepo.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo..");
        }
    }
}