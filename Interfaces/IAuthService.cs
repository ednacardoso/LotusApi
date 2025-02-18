using System.Threading.Tasks;

public interface IAuthService
{
    Task<AuthResultDto> Login(LoginRequest request);
    Task<AuthResultDto> RefreshToken(string token);
    Task<bool> Register(RegistrationRequest request);
}

