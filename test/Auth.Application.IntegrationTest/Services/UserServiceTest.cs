using System.Net.Http.Json;
using Auth.Api.Models.Responses;
using Auth.Application.Extensions;
using Auth.Application.IntegrationTest.Mocks;

namespace Auth.Application.IntegrationTest.Services;

public class UserServiceTest : BaseIntegrationTest
{
    private const string Url = "http://localhost:5139/user";
    
    public UserServiceTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact(DisplayName = "Create user - Should create user")]
    public async Task CreateAsync_ShouldReturnNewUserId()
    {
        // Arrange
        var request = UserMock.CreateUserRequest();

        // Act
        var response = await Client.PostAsJsonAsync(Url, request);
        var result = await response.Content.ReadAsJsonAsync<CreateEntityResponse>();

        // Assert
        Assert.NotNull(result?.Id);
        Assert.NotEqual(Guid.Empty, result.Id);
    }
}