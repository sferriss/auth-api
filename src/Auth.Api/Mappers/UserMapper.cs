using Auth.Api.Models.Requests;
using Auth.Api.Models.Responses;
using Auth.Domain.Dtos;
using Riok.Mapperly.Abstractions;

namespace Auth.Api.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial CreateOrUpdateUserDto ToDto(CreateUserRequest request);
    public partial GetUserResponse ToResponse(UserDto dto);
    public partial CreateOrUpdateUserDto ToDto(UpdateUserRequest request);
    public partial LoginDto ToDto(LoginRequest request);
}