using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class PasswordChangeDto
    {
        [Required] public string OldPassword { get; set; }
        [Required] public string NewPassword { get; set; }
        [Required] public string NewPasswordConfirm { get; set; }
    }
}