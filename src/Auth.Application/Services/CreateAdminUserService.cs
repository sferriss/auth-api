using Auth.Domain.Dtos;
using Auth.Domain.Repositories;
using Auth.Domain.Services;

namespace Auth.Application.Services;

public class CreateAdminUserService(IUserRepository userRepository, IUserService userService) : ICreateAdminUserService
{
    public async Task CreateUserAdminAsync()
    {
        var hasAnyUser = await userRepository.HasAnyAsync();
        if (hasAnyUser)
        {
            return;
        }
        
        var userAdmin = NewUserAdmin();
        await userService.CreateAsync(userAdmin);
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