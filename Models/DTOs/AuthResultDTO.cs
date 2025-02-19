using System;

namespace Lotus.Models.DTOs.Responses
{
    public class AuthResultDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public UserDto User { get; set; }
    }
}
