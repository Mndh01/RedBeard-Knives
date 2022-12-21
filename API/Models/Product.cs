using System.Collections.Generic;

namespace API.Models
{
    public class Product : BaseEntity
    {   
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public int SoldItems { get; set; }
        public ProductCategory Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}