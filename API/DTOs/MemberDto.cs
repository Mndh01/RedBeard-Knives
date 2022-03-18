using System.Collections.Generic;
using API.Models;

namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }
        public int Age { get; set;}
        public string Gender { get; set; }
        public IEnumerable<AddressDto> Addresses { get; set; }
    }
}