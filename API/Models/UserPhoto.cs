using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserPhoto : BaseEntity
    {
        public string Url { get; set; }
        public string PublicId { get; set; }
        public int UserId { get; set; }
    }
}