using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lotus.Interfaces;
using Lotus.Models.DTOs.Requests;
using Lotus.Exceptions;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.Login(request);
            return Ok(result);
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "administrador")]
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] RegistrationRequest request)
    {
        try
        {
            await _authService.Register(request);
            return Ok(new { message = "Usuário criado com sucesso." });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        await _authService.InitiatePasswordReset(request.Email);
        return Ok(new { message = "Se o email existir, instruções de recuperação foram enviadas." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        await _authService.ResetPassword(request);
        return Ok(new { message = "Senha alterada com sucesso." });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        try
        {
            await _authService.Register(request);
            return Ok(new { message = "Usuário registrado com sucesso." });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        try
        {
            var result = await _authService.RefreshToken(refreshToken);
            return Ok(result);
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "administrador")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok(new { message = "Bem-vindo, administrador!" });
    }

    [Authorize(Roles = "funcionario")]
    [HttpGet("funcionario-only")]
    public IActionResult FuncionarioOnly()
    {
        return Ok(new { message = "Área exclusiva para funcionários." });
    }

    [Authorize(Roles = "cliente")]
    [HttpGet("cliente-only")]
    public IActionResult ClienteOnly()
    {
        return Ok(new { message = "Bem-vindo, cliente!" });
    }
}
