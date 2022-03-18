using System;
using System.ComponentModel.DataAnnotations;
using API.Models;

namespace API.DTOs
{
    public class RegisterDto
    {
        [StringLength(20, MinimumLength = 8)]
        [Required] public string Password { get; set; }
        [Required] public string Username { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] [Phone] public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public UserPhoto Photo { get; set; }
        [Required] public Address Address { get; set; }
    }
}