using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IProductRepository
    {                
        Task<bool> SaveAllAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync(string type, double price, int inStock, int soldItems);
        Task<bool> AddProductAsync(Product product);
        bool DeleteProduct(int id);
        // Task<IEnumerable<Product>> GetProductsByPriceAsync(double price);
        // Task<IEnumerable<Product>> GetProductsByTypeAsync(string type);
    }
}