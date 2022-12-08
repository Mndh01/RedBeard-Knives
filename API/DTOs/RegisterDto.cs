using System;
using System.ComponentModel.DataAnnotations;
using API.Models;

namespace API.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string SureName { get; set; }
        [Required] [StringLength(20, MinimumLength = 8)] public string Password { get; set; }
        [Required] public string PasswordConfirm { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public UserPhoto Photo { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public Address Address { get; set; }
    }
}