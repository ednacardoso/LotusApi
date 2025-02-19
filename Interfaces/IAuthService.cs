using Lotus.Models.DTOs.Requests;

namespace Lotus.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> Login(LoginRequest request);
        Task<AuthResultDto> RefreshToken(string token);
        Task<bool> Register(RegistrationRequest request);
        Task<bool> InitiatePasswordReset(string email);
        Task<bool> ResetPassword(ResetPasswordRequest request);
    }
}
