using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public string PhotoUrl { get; set; }
        public int InStock { get; set; }
        public int SoldItems { get; set; }
        public ICollection<PhotoDto> Photo { get; set; }
    }
}