using BCrypt.Net;

public static class PasswordHelper
{
    public static string HashPassword(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public static bool VerifyPassword(string senhaDigitada, string senhaCriptografada)
    {
        return BCrypt.Net.BCrypt.Verify(senhaDigitada, senhaCriptografada);
    }
}
