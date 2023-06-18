using System.Collections.Generic;

namespace API.Models
{
    public class Address : BaseEntity
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress { get; set; }
        public string DisplayName { get; set; }
        public bool IsCurrent { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}