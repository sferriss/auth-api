using Auth.Application.Exceptions;
using Auth.Application.Mappers;
using Auth.Database.Abstractions;
using Auth.Domain.Dtos;
using Auth.Domain.Repositories;
using Auth.Domain.Services;

namespace Auth.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> CreateAsync(CreateOrUpdateUserDto userDto)
    {
        var anyWithLogin = await _userRepository.HasAnyWithLoginAsync(userDto.Login!);
        
        if (anyWithLogin)
        {
            throw new BusinessValidationException("Login already exists");
        }
        
        var anyWithEmail = await _userRepository.HasAnyWithEmailAsync(userDto.Email!);
        
        if (anyWithEmail)
        {
            throw new BusinessValidationException("Email already exists");
        }
        
        var anyWithPhoneNumber = await _userRepository.HasAnyWithPhoneNumberAsync(userDto.Contact!.PhoneNumber);
        
        if (anyWithPhoneNumber)
        {
            throw new BusinessValidationException("Phone number already exists");
        }
        
        var password = _passwordHasher.HashPassword(userDto.Password!);
        var newUser = UserMapper.ToDomain(userDto, password); 
        _userRepository.Add(newUser);

        await _unitOfWork.CommitAsync();
        return newUser.Id;
    }

    public async Task UpdateAsync(Guid id, CreateOrUpdateUserDto userDto)
    {
        var user = await _userRepository.GetByIdWithContactsAsync(id);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        user.Update(userDto);
        
        var password = userDto.Password != null ? _passwordHasher.HashPassword(userDto.Password!) : null;
        user.UpdatePassword(password);
        
        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task<UserDto> GetAsync(Guid id)
    {
        var result = await _userRepository.GetByIdWithContactsAsync(id);

        if (result is null)
        {
            throw new NotFoundException("User not found");
        }

        return UserMapper.ToDto(result);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdWithContactsAsync(id);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        _userRepository.Remove(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task<UserDto[]> GetAsync()
    {
        var users = await _userRepository.GetAllContactsAsync();
        
        if (users is null || !users.Any())
        {
            throw new NotFoundException("User not found");
        }

        return users
            .Select(UserMapper.ToDto)
            .ToArray();
    }
}