namespace Auth.Api.Models.Responses;

public record GetUserResponse(
    Guid Id,
    string Name,
    string Email,
    string Login,
    UserContactResponse Contact);