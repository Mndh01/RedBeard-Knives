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
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController (IUnitOfWork unitOfWork, IPhotoService photoService, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }
        
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {   
            var spec = new ProductsWithTypesSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

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

            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);

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
            var categories = await _unitOfWork.Repository<ProductCategory>().ListAllAsync();

            if (categories != null) return Ok(categories);

            return BadRequest("Couldn't find categories...");
        }
        
        // Set authorization only for admin
        [HttpPost("add-category")]
        public async Task<ActionResult<ProductCategory>> AddCategory(ProductCategory newCategory)
        {
            BaseSpecification<ProductCategory> spec = new BaseSpecification<ProductCategory>(pc => pc.Name == newCategory.Name.ToLower());
            
            var exists = await _unitOfWork.Repository<ProductCategory>().GetEntityWithSpec(spec);

            if(exists != null) { return BadRequest("Category already exists..."); }
            
            var category = new ProductCategory { Name = newCategory.Name.ToLower() };

            _unitOfWork.Repository<ProductCategory>().Add(category);

            if (await _unitOfWork.Complete() > 0) return Ok(category);

            return BadRequest("Couldn't add category...");
        }

        // Set authorization only for admin
        [HttpPost("add-product")]
        public async Task<ActionResult<Product>> AddProduct(Product product) 
        {
            var newProduct  = _mapper.Map<Product>(product);
            
            newProduct.Name = newProduct.Name.ToLower();

            BaseSpecification<Product> spec = new BaseSpecification<Product>(p => p.Name == product.Name.ToLower());

            newProduct.Category = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(product.Category.Id);
            
            if (await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec) != null) return BadRequest("Name already exists for another product..");

            _unitOfWork.Repository<Product>().Add(newProduct);

            if(await _unitOfWork.Complete() > 0) return Created(new Uri($"{Request.Path}/{newProduct.Id}", UriKind.Relative) ,newProduct);

            return BadRequest("Failed to add product");               
        }
        
        // Set authorization only for admin
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file, int id)
        {
            
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

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

            if (await _unitOfWork.Complete() > 0) return CreatedAtRoute("GetProduct", new {id = product.Id}, _mapper.Map<PhotoDto>(photo));

            return BadRequest("Failed to add the photo..");
        }

        // Set authorization only for admin
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {   
            _unitOfWork.Repository<Product>().Delete(new Product {Id = id});

            if (await _unitOfWork.Complete() > 0) return NoContent();
            
            return BadRequest("Failed to remove product..");
        }

        // Set authorization only for admin
        [HttpDelete("delete-photo")]
        public async Task<ActionResult<Product>> DeletePhoto(int ProductId, int PhotoId)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(ProductId);

            var photo = product.Photos.FirstOrDefault(Photo => Photo.Id == PhotoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete the main photo");

            if (photo.PublicId != null) {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }
            
            product.Photos.Remove(photo);

            if (await _unitOfWork.Complete() > 0) return NoContent();

            return BadRequest("Failed to delete the photo..");
        }
        
        // Set authorization only for admin
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductUpdateDto productUpdateDto) 
        {
            productUpdateDto.Name = productUpdateDto.Name.ToLower();

            productUpdateDto.Category.Name = productUpdateDto.Category.Name.ToLower();
            
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productUpdateDto.Id);

            _mapper.Map(productUpdateDto, product);

            _unitOfWork.Repository<Product>().Update(product);

            if (await _unitOfWork.Complete() > 0) return NoContent();

            return BadRequest("Failed to update product..");
        }

        // Set authorization only for admin
        [HttpPut("set-main-photo")]
        public async Task<ActionResult> SetMainPhoto(int ProductId, int PhotoId)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(ProductId);

            var photo = product.Photos.FirstOrDefault(photo => photo.Id == PhotoId);

            if (photo.IsMain) return BadRequest("This is already the main photo");

            var currentMain = product.Photos.FirstOrDefault(photo => photo.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            
            photo.IsMain = true;

            if (await _unitOfWork.Complete() > 0) return NoContent();

            return BadRequest("Failed to set main photo..");
        }
    }
}