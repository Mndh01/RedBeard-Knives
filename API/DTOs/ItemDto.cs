using System.Collections.Generic;
using API.Models;

namespace API.DTOs
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public int Price { get; set; }
        public int InStock { get; set; }
        public int SoldItems { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}