using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        public ProductsController (IProductRepository productRepository, IPhotoService photoService, IMapper mapper) 
        {
            _mapper = mapper;
            _photoService = photoService;
            _productRepository = productRepository;
        }
        
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<ItemDto>> GetProductById(int id) 
        {   
            var product = await _productRepository.GetItemAsync(id);  

            if (product != null)
            {
                return Ok(product);
            }
            return BadRequest("Couldn't find product...");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string category, int price=-1, int inStock=-1, int soldItems=-1) 
        {
            var products = await _productRepository.GetItemsAsync(category, price, inStock, soldItems);

            if (products != null) 
                return Ok(products);
            
            return BadRequest("Couldn't find products with such properties...");
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

            if (await _productRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetProduct", new {id = product.Id}, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding the photo...");
        }

        
        [HttpPost("add-product")]
        public async Task<ActionResult<Product>> AddProduct(Product product) 
        {
            var newProduct  = _mapper.Map<Product>(product);
            newProduct.Category = newProduct.Category.ToLower();
            
            if(await _productRepository.AddProductAsync(newProduct))
                return Ok(newProduct);

            return BadRequest("Failed to add product");               
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var result = _productRepository.DeleteProduct(id);

            if (result)
                return Ok("Deleted successfully");
            
            return BadRequest("Failed to remove product");
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

            return BadRequest("Failed to delete the photo");
        }
        
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductUpdateDto productUpdateDto) 
        {
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

            return BadRequest("Failed to set main photo");
        }
    }
}