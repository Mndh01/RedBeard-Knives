using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Product
    {   
        public int Id { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public int InStock { get; set; }
        public int SoldItems { get; set; }
    }
}