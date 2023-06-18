using System;

namespace API.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }
    }
}