﻿using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System;

public class AuthService : IAuthService
{
    private readonly MLotusContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(MLotusContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResultDto> Login(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.SenhaHash, user.SenhaHash))
            throw new UnauthorizedException("Usuário ou senha incorretos");

        var (token, expires) = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken(user.Email);

        await SaveRefreshToken(refreshToken);

        return new AuthResultDto
        {
            Token = token,
            Expires = expires,
            RefreshToken = refreshToken.Token,
            User = new UserDto
            {
                UserId = user.UserId,
                Nome = user.Nome,
                Tipo = user.Tipo
            }
        };
    }

    public async Task<AuthResultDto> RefreshToken(string token)
    {
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == token);

        if (storedToken == null || storedToken.Expiration < DateTime.UtcNow)
            throw new UnauthorizedException("Token inválido ou expirado");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == storedToken.UserEmail);

        if (user == null)
            throw new UnauthorizedException("Usuário não encontrado");

        var (newToken, expires) = GenerateJwtToken(user);

        return new AuthResultDto
        {
            Token = newToken,
            Expires = expires,
            User = new UserDto
            {
                UserId = user.UserId,
                Nome = user.Nome,
                Tipo = user.Tipo
            }
        };
    }

    public async Task<bool> Register(RegistrationRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new ValidationException("E-mail já cadastrado");

        var user = new User
        {
            Nome = request.Nome,
            Email = request.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
            Tipo = request.Tipo
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return true;
    }

    private (string token, DateTime expires) GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
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

    private RefreshToken GenerateRefreshToken(string userEmail)
    {
        return new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserEmail = userEmail,
            Expiration = DateTime.UtcNow.AddDays(7)
        };
    }

    private async Task SaveRefreshToken(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
    }
}
