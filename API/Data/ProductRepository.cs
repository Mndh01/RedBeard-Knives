using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using API.Helpers;
using System;

namespace API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(product => product.Id == id);
        }
        public async Task<Product> CheckProductExistsByName(string name)
        {
            return await _context.Products
            .FirstOrDefaultAsync(product => product.Name == name);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string category, int price, int inStock, int soldItems) 
        {
            var result = await _context.Products
            .Include(p => p.Photos)
            .ToListAsync();

            if (!string.IsNullOrEmpty(category))
                result = result.Where(p => p.Category.Name == category).ToList();

            if (price != -1)
                result = result.Where(p => p.Price <= price).ToList();

            if (inStock != -1)
                result = result.Where(p => p.InStock == inStock).ToList();

            if (soldItems != -1)
                result = result.Where(p => p.SoldItems == soldItems).ToList();

            return result;
                
        }

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

        public async Task<PagedList<ItemDto>> GetItemsAsync(ProductParams productParams, string category, int price, int inStock, int soldItems)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (price != -1)
                query = query.Where(p => p.Price <= price);

            if (inStock != -1)
                query = query.Where(p => p.InStock == inStock);

            if (soldItems != -1)
                query = query.Where(p => p.SoldItems == soldItems);

            return await PagedList<ItemDto>.CreateAsync(query, productParams.PageNumber, productParams.PageSize);
        }

        public async Task<ItemDto> GetItemAsync(int id)
        {
            return await _context.Products
                .Where(product => product.Id == id)
                .Include(p => p.Category)
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
        
        public async Task<ItemDto> GetItemByNameAsync(string name)
        {
            return await _context.Products
            .Where(product => product.Name == name)
            .Include(p => p.Category)
            .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductCategory>> GetCategoriesAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }
        
        public async Task<ProductCategory> GetCategoryByNameAsync(string name)
        {
            return await _context.ProductCategories.Where(category => category.Name == name).FirstOrDefaultAsync();
        }
        
        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

    }
}