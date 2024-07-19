using System.Net;
using System.Net.Http.Json;
using Auth.Api.ExceptionHandlers.Responses;
using Auth.Api.Models.Requests;
using Auth.Api.Models.Responses;
using Auth.Application.Extensions;
using Auth.Application.IntegrationTest.Mocks;

namespace Auth.Application.IntegrationTest.Services;

public class UserServiceTest : BaseIntegrationTest
{
    public UserServiceTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    
    [Fact(DisplayName = "Create user - Should create new user")]
    public async Task CreateAsync_ShouldCreateNewUser()
    {
        // Arrange
        var request = UserMock.CreateUserRequest();

        // Act
        await AuthorizeAsync();
        var response = await Client.PostAsJsonAsync(UrlUser, request);
        var result = await response.Content.ReadAsJsonAsync<CreateEntityResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotEqual(Guid.Empty, result?.Id);
    }
    
    [Fact(DisplayName = "Create user - Should return unauthorized")]
    public async Task CreateAsync_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = UserMock.CreateUserRequest();

        // Act
        await AuthorizeAsync("admin", "admin2");
        var response = await Client.PostAsJsonAsync(UrlUser, request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact(DisplayName = "Create user - Should return bad request when login already exists")]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenLoginAlreadyExists()
    {
        // Arrange
        var request = new CreateUserRequest(
            "John Doe",
            "johndoe1@example.com",
            "john1",
            "stringadasdasd",
            new UserContactRequest("00123456489"));
        
        var request2 = new CreateUserRequest(
            "John Doe",
            "johndoe1@example.com",
            "john1",
            "stringadasdasd",
            new UserContactRequest("00123456489"));

        // Act
        await AuthorizeAsync();
        await Client.PostAsJsonAsync(UrlUser, request);
        var response = await Client.PostAsJsonAsync(UrlUser, request2);
        var result = await response.Content.ReadAsJsonAsync<ExceptionResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Login already exists", result?.Title);
    }
    
    [Fact(DisplayName = "Create user - Should return bad request when email already exists")]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenEmailAlreadyExists()
    {
        // Arrange
        var request = new CreateUserRequest(
            "John Doe",
            "johndoe2@example.com",
            "john3",
            "stringadasdasd",
            new UserContactRequest("00123356789"));
        
        var request2 = new CreateUserRequest(
            "John Doe",
            "johndoe2@example.com",
            "john45",
            "stringadasdasd",
            new UserContactRequest("00123356789"));

        // Act
        await AuthorizeAsync();
        await Client.PostAsJsonAsync(UrlUser, request);
        var response = await Client.PostAsJsonAsync(UrlUser, request2);
        var result = await response.Content.ReadAsJsonAsync<ExceptionResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Email already exists", result?.Title);
    }
    
    [Fact(DisplayName = "Create user - Should return bad request when phone number already exists")]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenPhoneNumberAlreadyExists()
    {
        // Arrange
        var request = new CreateUserRequest(
            "John Doe",
            "john13@example.com",
            "john32",
            "stringadasdasd",
            new UserContactRequest("00123656789"));
        
        var request2 = new CreateUserRequest(
            "John Doe",
            "john@example.com",
            "john",
            "stringadasdasd",
            new UserContactRequest("00123656789"));

        // Act
        await AuthorizeAsync();
        await Client.PostAsJsonAsync(UrlUser, request);
        var response = await Client.PostAsJsonAsync(UrlUser, request2);
        var result = await response.Content.ReadAsJsonAsync<ExceptionResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Phone number already exists", result?.Title);
    }
    
    [Theory(DisplayName = "Create user - Should return bad request when invalid request")]
    [InlineData("", "john@example.com", "john", "stringadasdasd", "00123656789")]
    [InlineData("John Doe", "", "john", "stringadasdasd", "00123656789")]
    [InlineData("John Doe", "john@example.com", "", "stringadasdasd", "00123656789")]
    [InlineData("John Doe", "john@example.com", "john", "", "00123656789")]
    [InlineData("John Doe", "john@example.com", "john", "stri", "00123656789")]
    [InlineData("John Doe", "john@example.com", "john", "stringadasdasd", "")]
    [InlineData("John Doe", "john@example.com", "john", "stringadasdasd", "00123656")]
    [InlineData("John Doe", "john@example.com", "john", "stringadasdasd", "0012365678923423")]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenInvalidRequest(
        string name,
        string email,
        string login,
        string password,
        string phoneNumber)
    {
        // Arrange
        var request = new CreateUserRequest(
            name,
            email,
            login,
            password,
            new UserContactRequest(phoneNumber));

        // Act
        await AuthorizeAsync();
        var response = await Client.PostAsJsonAsync(UrlUser, request);
        var result = await response.Content.ReadAsJsonAsync<ExceptionResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        if (string.IsNullOrEmpty(name))
        {
            Assert.True(result!.Errors!.ContainsKey("Name"));
        }
        if (string.IsNullOrEmpty(email))
        {
            Assert.True(result!.Errors!.ContainsKey("Email"));
        }
        if (string.IsNullOrEmpty(login))
        {
            Assert.True(result!.Errors!.ContainsKey("Login"));
        }
        if (string.IsNullOrEmpty(password) || password.Length < 8)
        {
            Assert.True(result!.Errors!.ContainsKey("Password"));
        }
        if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 11 || phoneNumber.Length > 12)
        {
            Assert.True(result!.Errors!.ContainsKey("Contact.PhoneNumber"));
        }
    }
}