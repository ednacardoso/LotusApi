using System;
using System.Text.Json.Serialization;

namespace Lotus.Models

{
    
    public class LoginRequest
    {
        public string Email { get; set; }
        public string SenhaHash { get; set; }
    }

}
