using System;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class Review : BaseEntity
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore] public Product Product { get; set; }
    }
}