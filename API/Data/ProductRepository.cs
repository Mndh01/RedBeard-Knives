using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(product => product.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string type, double price, int inStock, int soldItems) 
        {

            var result = await _context.Products
            .Include(p => p.Photos)
            .ToListAsync();

            if (!string.IsNullOrEmpty(type))
                result = result.Where(p => p.Type == type).ToList();

            if (price != -1)
                result = result.Where(p => p.Price <= price).ToList();

            if (inStock != -1)
                result = result.Where(p => p.InStock == inStock).ToList();

            if (soldItems != -1)
                result = result.Where(p => p.SoldItems == soldItems).ToList();

            return result;
                
        }
        // public async Task<IEnumerable<Product>> GetProductsByPriceAsync(double price)
        // {
        //     return await _context.Products
        //     .Where(p => p.Price <= price)
        //     .ToListAsync();
        // }

        // public async Task<IEnumerable<Product>> GetProductsByTypeAsync(string type)
        // {
        //     return await _context.Products
        //     .Where(p => p.Type == type)
        //     .ToListAsync();
        // }

        public async Task<bool> AddProductAsync(Product product)
        {
            _context.Products.Add(product);   
               return await SaveAllAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return true;
            }   
            
            return false;
        }
    }
}