using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;

namespace API.Interfaces
{
    public interface IProductRepository
    {                
        Task<bool> SaveAllAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CheckProductExistsByName(string name);
        Task<IEnumerable<Product>> GetProductsAsync(string type, int price, int inStock, int soldItems);
        Task<bool> AddProductAsync(Product product);
        bool DeleteProduct(int id);
        Task<PagedList<ItemDto>> GetItemsAsync(string category, int price, int inStock, int soldItems);
        Task<ItemDto> GetItemAsync(int id);
        Task<ItemDto> GetItemByNameAsync(string name);
        void Update(Product product);
    }
}