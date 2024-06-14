namespace Auth.Domain.Dtos;

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    string Login,
    ContactDto Contact);