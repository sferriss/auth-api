using Auth.Domain.Dtos;

namespace Auth.Domain.Services;

public interface ILoginService
{
    Task<string> LoginAsync(LoginDto loginDto);
}