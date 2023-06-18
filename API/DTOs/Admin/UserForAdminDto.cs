using System.Collections.Generic;

namespace API.DTOs.Admin
{
    public class UserForAdminDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string SureName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        // public string PhotosUrl { get; set; } // TODO: Consider adding this
        public string DateOfBirth { get; set; }
        public IReadOnlyCollection<AddressDto> Addresses { get; set; }
        public IReadOnlyCollection<OrderForAdminDto> Orders { get; set; }
        public IReadOnlyCollection<string> Roles { get; set; }

    }
}