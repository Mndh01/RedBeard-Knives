using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserUpdateDto
    {
        [Required] public string Username { get; set; }
        [Required] public string Surename { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] [StringLength(20, MinimumLength = 8)] public string Password { get; set; }
        [Required] public string PasswordConfirm { get; set; }
    }
}