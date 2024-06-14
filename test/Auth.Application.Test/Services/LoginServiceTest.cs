using Auth.Application.Exceptions;
using Auth.Application.Services;
using Auth.Application.Test.Mocks;
using Auth.Domain.Dtos;
using Auth.Domain.Entities;
using Auth.Domain.Repositories;
using Auth.Domain.Services;
using Moq;

namespace Auth.Application.Test.Services;

public class LoginServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly LoginService _loginService;

    public LoginServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _loginService = new LoginService(_userRepositoryMock.Object, _tokenServiceMock.Object, _passwordHasherMock.Object);
    }
    
    [Fact(DisplayName = "Login - Should login")]
    public async Task LoginAsync_ShouldReturnToken_WhenUserExists()
    {
        // Arrange
        var loginDto = new LoginDto("john_doe", "stringadasdasd");
        var existingUser = UserMock.ExistingUser();
        
        _userRepositoryMock.Setup(repo => repo.GetByLoginAsync(loginDto.Login))
            .ReturnsAsync(existingUser);
        
        _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(existingUser.Password, loginDto.Password))
            .Returns(true);
        
        _tokenServiceMock.Setup(service => service.GenerateToken(existingUser))
            .Returns("mocked_token");

        // Act
        var token = await _loginService.LoginAsync(loginDto);

        // Assert
        Assert.Equal("mocked_token", token);
    }

    [Fact(DisplayName = "Login - Should throw exception when user not found")]
    public async Task LoginAsync_ShouldThrowUnauthorizedException_WhenUserDoesNotExist()
    {
        // Arrange
        var loginDto = new LoginDto("user", "Password123");

        _userRepositoryMock.Setup(x => x.GetByLoginAsync(loginDto.Login)).ReturnsAsync((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _loginService.LoginAsync(loginDto));
    }
}