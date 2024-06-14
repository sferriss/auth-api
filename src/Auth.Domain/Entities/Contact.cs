using Auth.Domain.Abstractions;
using Auth.Domain.Dtos;

namespace Auth.Domain.Entities;

public sealed class Contact : IEntity
{
    public Guid Id { get; init; }
    
    public string PhoneNumber { get; private set; }
    
    private Contact(){}
    
    public Contact(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
    
    public void Update(CreateOrUpdateContactDto contactDto)
    {
        PhoneNumber = contactDto.PhoneNumber;
    }
}