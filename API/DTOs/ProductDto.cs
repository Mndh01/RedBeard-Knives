using System.Collections.Generic;

namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public string PhotoUrl { get; set; }
        public int InStock { get; set; }
        public int SoldItems { get; set; }
        public ICollection<PhotoDto> Photo { get; set; }
    }
}