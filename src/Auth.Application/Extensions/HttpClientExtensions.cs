using System.Text.Json;
namespace Auth.Application.Extensions;

public static class HttpClientExtensions
{
    public static async Task<T?> ReadAsJsonAsync<T>(this HttpContent content)
    {
        if (content == null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        var contentResponse = await content.ReadAsStringAsync();
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        return string.IsNullOrEmpty(contentResponse) ? default : JsonSerializer.Deserialize<T>(contentResponse, options);
    }
}