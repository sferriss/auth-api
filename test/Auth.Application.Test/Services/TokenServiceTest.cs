using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Auth.Application.Services;
using Auth.Application.Settings;
using Auth.Application.Test.Mocks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Test.Services;

public class TokenServiceTest
{
    private readonly TokenService _tokenService;
    private readonly ApplicationSettings _settings;

    public TokenServiceTest()
    {
        _settings = new ApplicationSettings
        {
            JwtSecret = "stringadasdasdhghghkjhkuhiyugyuouuhayisgdiahsoidhadasfs"
        };
        
        _tokenService = new TokenService(Options.Create(_settings));
    }
    
    [Fact(DisplayName = "Generate token - Should generate token")]
    public void GenerateToken_ShouldGenerateValidToken()
    {
        // Arrange
        var user = UserMock.ExistingUser();

        // Act
        var token = _tokenService.GenerateToken(user);

        // Assert
        Assert.NotNull(token);

        // Verificar se o token pode ser lido e validado
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.JwtSecret);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

        var jwtToken = validatedToken as JwtSecurityToken;
        Assert.NotNull(jwtToken);
    }
}