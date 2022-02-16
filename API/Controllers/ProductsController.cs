using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{   
    public class Products : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        public Products (IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }
        
        [HttpGet("byId/{id}")]
        public async Task<Product> GetProductById(int id) 
        {   
            var product = await _productRepository.GetProductByIdAsync(id);  

            return product;
            
        }
    
        [HttpGet("byPrice/{price}")]
        public async Task<IEnumerable<Product>> GetProductsByPrice(double price) 
        {
            var products  = await _productRepository.GetProductsByPriceAsync(price);  

            return products.Cast<Product>().ToArray();
           
        }
        
        [HttpGet("byType/{type}")]
        public async Task<IEnumerable<Product>> GetProductsByType(string type) 
        {
            var products = await _productRepository.GetProductsByTypeAsync(type); 
            
            return products.Cast<Product>().ToArray(); 
        }
        
    
    }
}