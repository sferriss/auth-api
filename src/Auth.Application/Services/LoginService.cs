using Auth.Application.Exceptions;
using Auth.Domain.Dtos;
using Auth.Domain.Repositories;
using Auth.Domain.Services;

namespace Auth.Application.Services;

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginService(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByLoginAsync(loginDto.Login);

        if (user is null || !_passwordHasher.VerifyPassword(user.Password, loginDto.Password))
        {
            throw new UnauthorizedException();
        }
        
        return _tokenService.GenerateToken(user);
    }
}