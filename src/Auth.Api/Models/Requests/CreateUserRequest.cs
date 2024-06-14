namespace Auth.Api.Models.Requests;

public record CreateUserRequest(
    string Name,
    string Email,
    string Login,
    string Password,
    UserContactRequest Contact);