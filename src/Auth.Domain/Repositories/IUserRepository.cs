using Auth.Domain.Abstractions;
using Auth.Domain.Entities;

namespace Auth.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByIdWithContactsAsync(Guid id);
    
    Task<bool> HasAnyWithLoginAsync(string login);
    
    Task<bool> HasAnyWithEmailAsync(string email);
    
    Task<bool> HasAnyWithPhoneNumberAsync(string phoneNumber);
    
    Task<User[]> GetAllContactsAsync();
    
    Task<User?> GetByLoginAsync(string login);
    
    Task<bool> HasAnyAsync();
}