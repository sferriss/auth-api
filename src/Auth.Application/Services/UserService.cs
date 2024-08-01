using Auth.Application.Exceptions;
using Auth.Application.Mappers;
using Auth.Database.Abstractions;
using Auth.Domain.Dtos;
using Auth.Domain.Repositories;
using Auth.Domain.Services;

namespace Auth.Application.Services;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    : IUserService
{
    public async Task<Guid> CreateAsync(CreateOrUpdateUserDto userDto)
    {
        var anyWithLogin = await userRepository.HasAnyWithLoginAsync(userDto.Login!);
        
        if (anyWithLogin)
        {
            throw new BusinessValidationException("Login already exists");
        }
        
        var anyWithEmail = await userRepository.HasAnyWithEmailAsync(userDto.Email!);
        
        if (anyWithEmail)
        {
            throw new BusinessValidationException("Email already exists");
        }
        
        var anyWithPhoneNumber = await userRepository.HasAnyWithPhoneNumberAsync(userDto.Contact!.PhoneNumber);
        
        if (anyWithPhoneNumber)
        {
            throw new BusinessValidationException("Phone number already exists");
        }
        
        var password = passwordHasher.HashPassword(userDto.Password!);
        var newUser = UserMapper.ToDomain(userDto, password); 
        userRepository.Add(newUser);

        await unitOfWork.CommitAsync();
        return newUser.Id;
    }

    public async Task UpdateAsync(Guid id, CreateOrUpdateUserDto userDto)
    {
        var user = await userRepository.GetByIdWithContactsAsync(id);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        user.Update(userDto);
        
        var password = userDto.Password != null ? passwordHasher.HashPassword(userDto.Password!) : null;
        user.UpdatePassword(password);
        
        userRepository.Update(user);
        await unitOfWork.CommitAsync();
    }

    public async Task<UserDto> GetAsync(Guid id)
    {
        var result = await userRepository.GetByIdWithContactsAsync(id);

        if (result is null)
        {
            throw new NotFoundException("User not found");
        }

        return UserMapper.ToDto(result);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await userRepository.GetByIdWithContactsAsync(id);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        userRepository.Remove(user);
        await unitOfWork.CommitAsync();
    }

    public async Task<UserDto[]> ListAsync()
    {
        var users = await userRepository.GetAllContactsAsync();
        
        if (users is null || users.Length is 0)
        {
            throw new NotFoundException("Users not found");
        }

        return users
            .Select(UserMapper.ToDto)
            .ToArray();
    }
}