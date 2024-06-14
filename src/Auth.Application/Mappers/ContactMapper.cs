using Auth.Domain.Dtos;
using Auth.Domain.Entities;

namespace Auth.Application.Mappers;

public static class ContactMapper
{
    public static Contact ToDomain(CreateOrUpdateContactDto contactDto)
    {
        return new Contact(contactDto.PhoneNumber);
    }
    
    public static ContactDto ToDto(Contact contact)
    {
        return new ContactDto(contact.Id, contact.PhoneNumber);
    }
}