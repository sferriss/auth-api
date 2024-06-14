using Auth.Domain.Entities;

namespace Auth.Domain.Services;

public interface ITokenService
{
    public string GenerateToken(User user);
}