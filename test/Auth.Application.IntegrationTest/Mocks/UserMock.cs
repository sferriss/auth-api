using Auth.Api.Models.Requests;

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
    
    public static UpdateUserRequest UpdateUserRequest()
    {
        return new UpdateUserRequest(
            "Johny Doe",
            "john_doe@email.com",
            "john.doe",
            "Abc12345",
            new UserContactRequest("51999999999")
        );
    }
}