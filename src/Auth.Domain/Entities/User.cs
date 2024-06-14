using Auth.Domain.Abstractions;
using Auth.Domain.Dtos;

namespace Auth.Domain.Entities;

public sealed class User : IEntity
{
    public Guid Id { get; init; }

    public string Name { get; private set; }
    
    public string Email { get; private set; }
    
    public string Login { get; private set; }

    public string Password { get; private set; }
    
    public Contact Contact { get; private set; }
    
    private User(){}

    public User(string name, string email, string login, string password, Contact contact)
    {
        Name = name;
        Email = email;
        Login = login;
        Password = password;
        Contact = contact;
    }

    public void Update(CreateOrUpdateUserDto userDto)
    {
        Name = userDto.Name ?? Name;
        Email = userDto.Email ?? Email;
        Login = userDto.Login ?? Login;
        
        if (userDto.Contact is not null)
        {
            Contact.Update(userDto.Contact);
        }
    }
    
    public void UpdatePassword(string? password)
    {
        Password = password ?? Password;
    }
}