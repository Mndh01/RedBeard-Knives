using System.Collections.Generic;

namespace API.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string AddressParts { get; set; }
        public bool IsCurrent { get; set; }
        public ICollection<UserAddresses> UserAddresses { get; set; }
    }
}