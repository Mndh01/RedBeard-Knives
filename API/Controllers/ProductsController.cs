using System.Collections.Generic;
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
        
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var product = await _productRepository.GetProductByIdAsync(1);

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
                return CreatedAtRoute("GetProduct", 1, _mapper.Map<PhotoDto>(photo));

            }

            return BadRequest("Problem adding the photo...");

        }
    }
}