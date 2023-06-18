using System.Collections.Generic;
using API.Models;

namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductCategory Category { get; set; }
        public string CategoryJson { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public int Price { get; set; }
        public int InStock { get; set; }
        public int SoldItems { get; set; }
        public ICollection<PhotoDto> Photos { get; set; } = new List<PhotoDto>();
        public ICollection<Review> Reviews { get; set; }  = new List<Review>();
    }
}

