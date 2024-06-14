using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Application.Settings;
using Auth.Domain.Entities;
using Auth.Domain.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Services;

public class TokenService : ITokenService
{
    private readonly ApplicationSettings _settings;

    public TokenService(IOptions<ApplicationSettings> settings)
    {
        _settings = settings.Value;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.JwtSecret);

        var tokenDescriptor = CreateTokenDescriptor(user, key);

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static SecurityTokenDescriptor CreateTokenDescriptor(User user, byte[] key)
    {
        return new SecurityTokenDescriptor()
        {
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity( new []
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("login", user.Login)
            })
        };
    }
}