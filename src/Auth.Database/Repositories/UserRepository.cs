using Auth.Database.Abstractions;
using Auth.Database.Contexts;
using Auth.Domain.Entities;
using Auth.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Auth.Database.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(UserAuthContext authContext) : base(authContext)
    {
    }

    public Task<User?> GetByIdWithContactsAsync(Guid id)
    {
        return
            GetQuery()
                .Include(x => x.Contact)
                .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<bool> HasAnyWithLoginAsync(string login)
    {
        return
            GetQuery()
                .AnyAsync(x => x.Login.ToLower() == login.ToLower());

    }

    public Task<bool> HasAnyWithEmailAsync(string email)
    {
        return
            GetQuery()
                .AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }

    public Task<bool> HasAnyWithPhoneNumberAsync(string phoneNumber)
    {
        return
            GetQuery()
                .AnyAsync(x => x.Contact.PhoneNumber
                    .Equals(phoneNumber));
    }

    public Task<User[]> GetAllContactsAsync()
    {
        return
            GetQuery()
                .AsNoTracking()
                .Include(x => x.Contact)
                .ToArrayAsync();
    }

    public Task<User?> GetByLoginAsync(string login)
    {
        return
            GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => 
                    x.Login.ToLower() == login.ToLower());
    }

    public Task<bool> HasAnyAsync()
    {
        return
            GetQuery()
                .AnyAsync();
    }
}