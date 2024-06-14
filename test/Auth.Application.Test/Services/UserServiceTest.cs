using Auth.Application.Exceptions;
using Auth.Application.Services;
using Auth.Application.Test.Mocks;
using Auth.Database.Abstractions;
using Auth.Domain.Entities;
using Auth.Domain.Repositories;
using Auth.Domain.Services;
using Bogus;
using Moq;

namespace Auth.Application.Test.Services;

public class UserServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UserService _userService;

    public UserServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        Mock<IPasswordHasher> passwordHasherMock = new();
        _userService = new UserService(_userRepositoryMock.Object, _unitOfWorkMock.Object, passwordHasherMock.Object);
    }

    [Fact(DisplayName = "Create user - Should create user")]
    public async Task CreateAsync_ShouldReturnNewUserId()
    {
        // Arrange
        var userDto = UserMock.CreateUserDto();
        
        _userRepositoryMock.Setup(x => x.HasAnyWithLoginAsync(userDto.Login!)).ReturnsAsync(false);
        _userRepositoryMock.Setup(x => x.HasAnyWithEmailAsync(userDto.Email!)).ReturnsAsync(false);
        _userRepositoryMock.Setup(x => x.HasAnyWithPhoneNumberAsync(userDto.Contact!.PhoneNumber)).ReturnsAsync(false);

        _userRepositoryMock.Setup(x => x.Add(It.IsAny<User>())).Verifiable();
        _unitOfWorkMock.Setup(x => x.CommitAsync());

        // Act
        await _userService.CreateAsync(userDto);

        // Assert
        _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
    }
    
    [Fact(DisplayName = "Create user - Should throw exception when login already exists")]
    public async Task CreateAsync_ShouldThrowException_WhenLoginAlreadyExists()
    {
        // Arrange
        var userDto = UserMock.CreateUserDto();

        _userRepositoryMock.Setup(x => x.HasAnyWithLoginAsync(userDto.Login!)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessValidationException>(() => _userService.CreateAsync(userDto));
    }
    
    [Fact(DisplayName = "Create user - Should throw exception when email already exists")]
    public async Task CreateAsync_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var userDto = UserMock.CreateUserDto();

        _userRepositoryMock.Setup(x => x.HasAnyWithLoginAsync(userDto.Login!)).ReturnsAsync(false);
        _userRepositoryMock.Setup(x => x.HasAnyWithEmailAsync(userDto.Email!)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessValidationException>(() => _userService.CreateAsync(userDto));
    }
    
    [Fact(DisplayName = "Create user - Should throw exception when phone number already exists")]
    public async Task CreateAsync_ShouldThrowException_WhenPhoneNumberAlreadyExists()
    {
        // Arrange
        var userDto = UserMock.CreateUserDto();

        _userRepositoryMock.Setup(x => x.HasAnyWithLoginAsync(userDto.Login!)).ReturnsAsync(false);
        _userRepositoryMock.Setup(x => x.HasAnyWithEmailAsync(userDto.Email!)).ReturnsAsync(false);
        _userRepositoryMock.Setup(x => x.HasAnyWithPhoneNumberAsync(userDto.Contact!.PhoneNumber)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessValidationException>(() => _userService.CreateAsync(userDto));
    }
    
    [Fact(DisplayName = "Update user - Should update user infos")]
    public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userDto = UserMock.UpdateUserDto();
        var existingUser = UserMock.ExistingUser();

        _userRepositoryMock.Setup(x => x.GetByIdWithContactsAsync(userId)).ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(x => x.Update(existingUser)).Verifiable();
        _unitOfWorkMock.Setup(x => x.CommitAsync());

        // Act
        await _userService.UpdateAsync(userId, userDto);

        // Assert
        _userRepositoryMock.Verify(x => x.Update(existingUser), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);

        Assert.Equal(userDto.Name, existingUser.Name);
        Assert.Equal(userDto.Login, existingUser.Login);
        Assert.Equal(userDto.Email, existingUser.Email);
        Assert.Equal(userDto.Contact!.PhoneNumber, existingUser.Contact.PhoneNumber);
    }
    
    [Fact(DisplayName = "Update user - Should throw exception when user not found")]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userDto = UserMock.UpdateUserDto();

        _userRepositoryMock.Setup(x => x.GetByIdWithContactsAsync(userId)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _userService.UpdateAsync(userId, userDto));
    }

    [Fact(DisplayName = "Get user - Should return user")]
    public async Task GetAsync_ShouldReturnUserDto_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = UserMock.ExistingUser();
        var expectedUserDto = UserMock.CreateUserDto();
        
        _userRepositoryMock.Setup(x => x.GetByIdWithContactsAsync(userId)).ReturnsAsync(existingUser);

        // Act
        var result = await _userService.GetAsync(userId);
        
        // Assert
        Assert.Equal(expectedUserDto.Login, result.Login);
    }
    
    [Fact(DisplayName = "Get user - Should throw exception when user not found")]
    public async Task GetAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock.Setup(x => x.GetByIdWithContactsAsync(userId)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _userService.GetAsync(userId));
    }
    
    [Fact(DisplayName = "Delete user - Should delete user")]
    public async Task DeleteAsync_ShouldRemoveUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = UserMock.ExistingUser();

        _userRepositoryMock.Setup(x => x.GetByIdWithContactsAsync(userId)).ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(x => x.Remove(existingUser)).Verifiable();
        _unitOfWorkMock.Setup(x => x.CommitAsync());

        // Act
        await _userService.DeleteAsync(userId);

        // Assert
        _userRepositoryMock.Verify(x => x.Remove(existingUser), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
    }
    
    [Fact(DisplayName = "Delete user - Should throw exception when user not found")]
    public async Task DeleteAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock.Setup(x => x.GetByIdWithContactsAsync(userId)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _userService.DeleteAsync(userId));
    }
    
    [Fact(DisplayName = "Get users - Should return all users")]
    public async Task GetAsync_ShouldReturnUserDtos_WhenUsersExist()
    {
        // Arrange
        var faker = new Faker<User>()
            .CustomInstantiator(x => new User(
                x.Random.String(),
                x.Internet.Email(),
                x.Random.String(),
                x.Random.String(),
                new Contact (x.Phone.PhoneNumber())));

        var users = faker.Generate(10);

        _userRepositoryMock.Setup(x => x.GetAllContactsAsync()).ReturnsAsync(users.ToArray);

        // Act
        var result = await _userService.GetAsync();

        // Assert
        Assert.Equal(10, result.Length);
    }
    
    [Fact(DisplayName = "Get users - Should throw exception when users not found")]
    public async Task GetAsync_ShouldThrowNotFoundException_WhenNoUsersExist()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetAllContactsAsync())!.ReturnsAsync((User[])null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _userService.GetAsync());
    }

    [Fact(DisplayName = "Get users - Should throw exception when users not found")]
    public async Task GetAsync_ShouldThrowNotFoundException_WhenUsersListIsEmpty()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetAllContactsAsync()).ReturnsAsync(Array.Empty<User>());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _userService.GetAsync());
    }
}