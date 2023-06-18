using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string SureName { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<AddressDto> Addresses { get; set; }
    }
}