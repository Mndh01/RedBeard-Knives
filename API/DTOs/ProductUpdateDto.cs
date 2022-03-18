using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public int InStock { get; set; }
        public int SoldItems { get; set; }
    }
}