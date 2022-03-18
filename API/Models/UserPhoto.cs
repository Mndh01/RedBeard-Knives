using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserPhoto
    {
        public string Url { get; set; }
        public string PublicId { get; set; }
        [Key] public int UserId { get; set; }
    }
}