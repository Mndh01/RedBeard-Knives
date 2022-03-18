using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public UserPhoto Photo { get; set; }
        public ICollection<UserAddresses> UserAddresses { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }

    }
}