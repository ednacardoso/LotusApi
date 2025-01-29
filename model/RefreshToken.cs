using System;

namespace Lotus.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserEmail { get; set; }
        public DateTime Expiration { get; set; }
    }
}
