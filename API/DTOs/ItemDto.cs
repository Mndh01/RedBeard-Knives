using System.Collections.Generic;

namespace API.DTOs
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string PhotoUrl { get; set; }
        public int Price { get; set; }
        public int InStock { get; set; }
        public int SoldItems { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
    }
}