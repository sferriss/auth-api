using Auth.Application.Exceptions;
using Auth.Domain.Dtos;
using Auth.Domain.Repositories;
using Auth.Domain.Services;

namespace Auth.Application.Services;

public class LoginService(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
    : ILoginService
{
    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetByLoginAsync(loginDto.Login);

        if (user is null || !passwordHasher.VerifyPassword(user.Password, loginDto.Password))
        {
            throw new UnauthorizedException();
        }
        
        return tokenService.GenerateToken(user);
    }
}