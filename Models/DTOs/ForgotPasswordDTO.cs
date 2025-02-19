using System.ComponentModel.DataAnnotations;

namespace Lotus.Models.DTOs.Requests
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
