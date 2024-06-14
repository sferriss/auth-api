namespace Auth.Api.ExceptionHandlers.Responses;

public class ExceptionResponse
{
    public string Type { get; init; } = null!;

    public string? Title { get; init; }

    public int Status { get; init; }

    public string TraceId { get; init; } = null!;
}