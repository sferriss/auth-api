using System.Net.Http.Headers;
using System.Net.Http.Json;
using Auth.Api.Models.Requests;
using Auth.Api.Models.Responses;
using Auth.Application.Extensions;

namespace Auth.Application.IntegrationTest;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient Client;
    private const string Url = "http://localhost:5139/login";
    protected const string UrlUser = "http://localhost:5139/user";
    private string? _token;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        Client = factory.CreateClient();
    }

    protected async Task AuthorizeAsync(string login = "admin", string password = "admin123")
    {
        if (_token is not null && login == "admin" && password == "admin123")
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            return;
        }
        
        var request = new LoginRequest(login, password);
        var response = await Client.PostAsJsonAsync(Url, request);
        var result = await response.Content.ReadAsJsonAsync<LoginResponse>();

        _token = result?.Token;
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
    }

    protected static string FormatUrl(object value)
    {
        return $"{UrlUser}/{value}";
    }
}