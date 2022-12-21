using System;

namespace API.Models
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}