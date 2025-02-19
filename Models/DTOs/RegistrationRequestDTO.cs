using System;

namespace Lotus.Models.DTOs.Requests
{
    public class RegistrationRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }
}
