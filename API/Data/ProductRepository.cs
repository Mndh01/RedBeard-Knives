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

        // public async Task<IEnumerable<Product>> GetProductsAsync(string type, double price) 
        // {
        //     return await _context.Products
        //         .Where(p => p.Price <= price? )
        // }
        public async Task<IEnumerable<Product>> GetProductsByPriceAsync(double price)
        {
            return await _context.Products
            .Where(p => p.Price <= price)
            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByTypeAsync(string type)
        {
            return await _context.Products
            .Where(p => p.Type == type)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}