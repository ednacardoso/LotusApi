using Lotus.Data;
using Lotus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace Lotus.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MLotusContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(MLotusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "E-mail já cadastrado." });
            }

            user.SenhaHash = BCrypt.Net.BCrypt.HashPassword(user.SenhaHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário registrado com sucesso." });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.SenhaHash, user.SenhaHash))
            {
                return Unauthorized(new { message = "Usuário ou senha incorretos" });
            }

            var (token, expires) = GenerateJwtToken(user);
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserEmail = user.Email,
                Expiration = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                token,
                expires,
                refreshToken = refreshToken.Token,
                user = new { user.Nome, user.Tipo }
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (storedToken == null || storedToken.Expiration < DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Token inválido ou expirado." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == storedToken.UserEmail);
            if (user == null)
            {
                return Unauthorized(new { message = "Usuário não encontrado." });
            }

            var (newToken, expires) = GenerateJwtToken(user);
            return Ok(new { token = newToken, expires });
        }

        private (string, DateTime) GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(3);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Role, user.Tipo)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
        }
    }
}
