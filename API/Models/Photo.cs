using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    [Table("Photos")]
    public class Photo : BaseEntity
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}