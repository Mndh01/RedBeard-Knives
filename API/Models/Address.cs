using System.Collections.Generic;

namespace API.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string AddressParts { get; set; }
        public bool IsCurrent { get; set; }
        public ICollection<UserAddresses> UserAddresses { get; set; }
    }
}