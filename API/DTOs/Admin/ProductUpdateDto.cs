using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.DTOs.Admin
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description {get; set; }
        public ProductCategory Category { get; set; }
        public int Price { get; set; }
        public int SoldItems { get; set; }
        public int InStock { get; set; }
    }
}