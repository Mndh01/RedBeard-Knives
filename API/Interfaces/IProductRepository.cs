using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Models;

namespace API.Interfaces
{
    public interface IProductRepository
    {                
        Task<bool> SaveAllAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync(string type, int price, int inStock, int soldItems);
        Task<IEnumerable<ProductCategory>> GetCategoriesAsync();
        Task<ProductCategory> GetCategoryByNameAsync(string name);
        Task<PagedList<ItemDto>> GetItemsAsync(ProductParams productParams, string category, int price, int inStock, int soldItems);
        Task<ItemDto> GetItemAsync(int id);
        Task<ItemDto> GetItemByNameAsync(string name);
        Task<Product> CheckProductExistsByName(string name);
        Task<bool> AddProductAsync(Product product);
        bool DeleteProduct(int id);
        void Update(Product product);
    }
}