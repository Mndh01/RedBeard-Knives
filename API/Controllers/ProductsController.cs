using System.Collections.Generic;
using System.Linq;
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
        private readonly IProductRepository _productRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        public ProductsController (IGenericRepository<Product> genProductRepo, IGenericRepository<ProductCategory> genCategoryRepo
            ,IProductRepository productRepository, IPhotoService photoService, IMapper mapper) 
        {
            _mapper = mapper;
            _photoService = photoService;
            _productRepository = productRepository;
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

            // Normal Repository Implementation below
            // var product = await _productRepository.GetItemAsync(id);

            // if (product != null)
            // {
            //     return Ok(product);
            // }
            // return BadRequest("Couldn't find product...");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery] ProductParams productParams, string category="", int price=-1, int inStock=-1, int soldItems=-1) 
        {
            var spec = new ProductsWithTypesSpecification();

            var products = await _genProductRepo.ListAsync(spec);

            if (products != null)
            {
                return Ok(products);
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

        [HttpPost("add-product")]
        public async Task<ActionResult<Product>> AddProduct(Product product) 
        {
            var newProduct  = _mapper.Map<Product>(product);
            
            newProduct.Name = newProduct.Name.ToLower();
            
            if (await _productRepository.CheckProductExistsByName(newProduct.Name) != null) return BadRequest("Name already exists for another product..");

            newProduct.Category.Name = newProduct.Category.Name.ToLower();
                        
            if(await _productRepository.AddProductAsync(newProduct))
                return Ok(newProduct);

            return BadRequest("Failed to add product");               
        }
        
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file, int id)
        {
            
            var product = await _productRepository.GetProductByIdAsync(id);

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

            if (await _productRepository.SaveAllAsync()) return CreatedAtRoute("GetProduct", new {id = product.Id}, _mapper.Map<PhotoDto>(photo));

            return BadRequest("Failed to add the photo..");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var result = _productRepository.DeleteProduct(id);

            if (result)
                return Ok("Deleted successfully.");
            
            return BadRequest("Failed to remove product..");
        }

        [HttpDelete("delete-photo")]
        public async Task<ActionResult<Product>> DeletePhoto(int ProductId, int PhotoId)
        {
            var product = await _productRepository.GetProductByIdAsync(ProductId);

            var photo = product.Photos.FirstOrDefault(Photo => Photo.Id == PhotoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete the main photo");

            if (photo.PublicId != null) {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }
            
            product.Photos.Remove(photo);

            if (await _productRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo..");
        }
        
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductUpdateDto productUpdateDto) 
        {
            productUpdateDto.Name = productUpdateDto.Name.ToLower();

            productUpdateDto.Category = productUpdateDto.Category.ToLower();
            
            var product = await _productRepository.GetProductByIdAsync(productUpdateDto.Id);

            _mapper.Map(productUpdateDto, product);

            _productRepository.Update(product);

            if (await _productRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update product..");
        }

        [HttpPut("set-main-photo")]
        public async Task<ActionResult> SetMainPhoto(int ProductId, int PhotoId)
        {
            var product = await _productRepository.GetProductByIdAsync(ProductId);

            var photo = product.Photos.FirstOrDefault(photo => photo.Id == PhotoId);

            if (photo.IsMain) return BadRequest("This is already the main photo");

            var currentMain = product.Photos.FirstOrDefault(photo => photo.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _productRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo..");
        }
    }
}