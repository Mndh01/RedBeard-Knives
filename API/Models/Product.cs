using System.Collections.Generic;

namespace API.Models
{
    public class Product
    {   
        public int Id { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public int InStock { get; set; }
        public int SoldItems { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}