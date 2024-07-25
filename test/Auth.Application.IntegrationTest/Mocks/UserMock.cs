using Auth.Api.Models.Requests;
using Auth.Domain.Dtos;
using Auth.Domain.Entities;

namespace Auth.Application.IntegrationTest.Mocks;

public static class UserMock
{
    public static CreateUserRequest CreateUserRequest()
    {
        return new CreateUserRequest(
            "John Doe",
            "johndoe@example.com",
            "john_doe",
            "stringadasdasd",
            new UserContactRequest("00123456789")
        );
    }
    
    public static CreateOrUpdateUserDto GetUserDto()
    {
        return new CreateOrUpdateUserDto(
            "Johnes Doe",
            "johnesdoe@example.com",
            "johnes_doe",
            "stringadasdasd",
            new CreateOrUpdateContactDto("98765432100")
        );
    }
    
    public static User ExistingUser()
    {
        return new User(
            "John Doe",
            "johndoe@example.com",
            "john_doe",
            "$2b$12$OT1zL.KNnbVTQGCOFfReEuxJWQpHKX7.xRYv9xm0KeV5UG.5jIBOq",
            new Contact("00123456789")
        );
    }
}