using System;
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
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id) 
        {   
            var product = await _productRepository.GetProductByIdAsync(id);  

            var productToReturn = _mapper.Map<ProductDto>(product);

            return Ok(productToReturn);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string type, double price=-1, int inStock=-1, int soldItems=-1) 
        {
            var products = await _productRepository.GetProductsAsync(type, price, inStock, soldItems);

            if (products != null) 
                return Ok(products);
            
            return BadRequest("Couldn't find such data!");
        }


        [HttpPost("add-product")]
        public async Task<ActionResult<Product>> AddProduct(Product product) 
        {
            var newProduct  = _mapper.Map<Product>(product);
            newProduct.Type.ToLower();
            
            if(await _productRepository.AddProductAsync(newProduct))
                return Ok(newProduct);

            return BadRequest("Failed to add product");               
        }

        [HttpDelete]
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
            var product = await _productRepository.GetProductByIdAsync(2);

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo 
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            product.Photos.Add(photo);

            if (await _productRepository.SaveAllAsync())
                return _mapper.Map<PhotoDto>(photo);

            return BadRequest("Problem setting the photo");

        }
    }
}
        // [HttpGet("byPrice/{price}")]
        // public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByPrice(double price) 
        // {
        //     var products  = await _productRepository.GetProductsByPriceAsync(price);  

        //     var ProductsToReturn = _mapper.Map<IEnumerable<ProductDto>>(products);

        //     return Ok(ProductsToReturn.ToList());
           
        // }
        
        // [HttpGet("byType/{type}")]
        // public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByType(string type) 
        // {
        //     var products = await _productRepository.GetProductsByTypeAsync(type); 
            
        //     var ProductsToReturn = _mapper.Map<IEnumerable<ProductDto>>(products);
            
        //     return Ok(ProductsToReturn); 
        // }