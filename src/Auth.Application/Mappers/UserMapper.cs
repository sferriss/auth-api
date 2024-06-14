using Auth.Domain.Dtos;
using Auth.Domain.Entities;

namespace Auth.Application.Mappers;

public static class UserMapper
{
    public static User ToDomain(CreateOrUpdateUserDto userDto, string password)
    {
        return new User(
            userDto.Name!,
            userDto.Email!,
            userDto.Login!,
            password,
            ContactMapper.ToDomain(userDto.Contact!));
    }
    
    public static UserDto ToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.Login,
            ContactMapper.ToDto(user.Contact));
    }
}