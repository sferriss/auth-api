namespace Auth.Api.Models.Requests;

public record UpdateUserRequest(
    string? Name,
    string? Email,
    string? Login,
    string? Password,
    UserContactRequest? Contact);