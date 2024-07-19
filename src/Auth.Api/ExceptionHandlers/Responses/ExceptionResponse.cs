using System.Text.Json.Serialization;

namespace Auth.Api.ExceptionHandlers.Responses;

public class ExceptionResponse
{
    public string Type { get; init; } = null!;
    
    public string? Title { get; init; }
    
    public int Status { get; init; }
    
    public string TraceId { get; init; } = null!;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IDictionary<string, string[]>? Errors { get; set; }
}