namespace Auth.Domain.Dtos;

public record CreateOrUpdateUserDto(
    string? Name,
    string? Email,
    string? Login,
    string? Password,
    CreateOrUpdateContactDto? Contact);