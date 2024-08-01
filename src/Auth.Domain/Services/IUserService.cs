using Auth.Domain.Dtos;

namespace Auth.Domain.Services;

public interface IUserService
{
    public Task<Guid> CreateAsync(CreateOrUpdateUserDto userDto);
    public Task UpdateAsync(Guid id, CreateOrUpdateUserDto userDto);
    public Task<UserDto> GetAsync(Guid id);
    public Task DeleteAsync(Guid id);
    public Task<UserDto[]> ListAsync();
}