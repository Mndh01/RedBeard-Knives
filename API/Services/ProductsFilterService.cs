using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ProductsFilterService
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public int InStock { get; set; }
        public int SoldItems { get; set; }
        public ProductsFilterService(int id, string type, int price, int inStock, int soldItems)
        {
            Id = id;
            Type = type;
            Price = price;
            InStock = inStock;
            SoldItems = soldItems;
        }
    }
}