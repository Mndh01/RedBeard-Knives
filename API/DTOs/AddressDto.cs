using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AddressDto
    {
        public int Id { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string State { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Street { get; set; }
        [Required] public string ZipCode { get; set; }
        [Required] public string DisplayName { get; set; }
        public string FullAddress { get; set; }
        public bool IsCurrent { get; set; }
    }
}