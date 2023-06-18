using System;
using System.Collections.Generic;
using API.Models.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using Stripe;

namespace API.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string SureName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string StripeCustomerId { get; set; }
        public UserPhoto Photo { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}