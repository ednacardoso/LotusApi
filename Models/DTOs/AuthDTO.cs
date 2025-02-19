using System;

public class AuthResultDto
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public string RefreshToken { get; set; }
    public UserDto User { get; set; }
}
