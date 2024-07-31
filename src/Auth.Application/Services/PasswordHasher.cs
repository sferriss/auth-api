using Auth.Application.Settings;
using Auth.Domain.Services;
using Microsoft.Extensions.Options;

namespace Auth.Application.Services;

public class PasswordHasher(IOptions<ApplicationSettings> applicationSettings) : IPasswordHasher
{
    private readonly ApplicationSettings _applicationSettings = applicationSettings.Value;

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, _applicationSettings.WorkFactor);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}