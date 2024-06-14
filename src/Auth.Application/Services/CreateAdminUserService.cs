using Auth.Domain.Dtos;
using Auth.Domain.Repositories;
using Auth.Domain.Services;

namespace Auth.Application.Services;

public class CreateAdminUserService : ICreateAdminUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public CreateAdminUserService(IUserRepository userRepository, IUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task CreateUserAdminAsync()
    {
        var hasAnyUser = await _userRepository.HasAnyAsync();
        if (hasAnyUser)
        {
            return;
        }
        
        var userAdmin = NewUserAdmin();
        await _userService.CreateAsync(userAdmin);
    }

    private static CreateOrUpdateUserDto NewUserAdmin()
    {
        return new CreateOrUpdateUserDto(
            "User Admin",
            "admin@email.com",
            "admin",
            "admin123",
            new CreateOrUpdateContactDto("99999999999")
        );
    }
}