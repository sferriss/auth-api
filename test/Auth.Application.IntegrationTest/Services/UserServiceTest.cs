using System.Net;
using System.Net.Http.Json;
using Auth.Api.ExceptionHandlers.Responses;
using Auth.Api.Models.Requests;
using Auth.Api.Models.Responses;
using Auth.Application.Extensions;
using Auth.Application.IntegrationTest.Mocks;

namespace Auth.Application.IntegrationTest.Services;

public class UserServiceTest(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
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
    
    [InlineData("", "john@example.com", "john", "stringadasdasd", "00123656789", "Name")]
    [InlineData("John Doe", "", "john", "stringadasdasd", "00123656789", "Email")]
    [InlineData("John Doe", "john@example.com", "", "stringadasdasd", "00123656789", "Login")]
    [InlineData("John Doe", "john@example.com", "john", "", "00123656789", "Password")]
    [InlineData("John Doe", "john@example.com", "john", "stri", "00123656789", "Password")]
    [InlineData("John Doe", "john@example.com", "john", "stringadasdasd", "", "Contact.PhoneNumber")]
    [InlineData("John Doe", "john@example.com", "john", "stringadasdasd", "00123656", "Contact.PhoneNumber")]
    [InlineData("John Doe", "john@example.com", "john", "stringadasdasd", "0012365678923423", "Contact.PhoneNumber")]
    [Theory(DisplayName = "Create user - Should return bad request when invalid request")]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenInvalidRequest(
        string name,
        string email,
        string login,
        string password,
        string phoneNumber,
        string expectedErrorMessage)
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
        Assert.True(result!.Errors!.ContainsKey(expectedErrorMessage));
    }
    
    [Fact(DisplayName = "Get user - Should return an user")]
    public async Task GetAsync_ShouldReturnAnUser()
    {
        // Arrange
        var request =  new CreateUserRequest(
            "John Doe",
            "johndoe2@example.com",
            "john2",
            "stringadasdasd",
            new UserContactRequest("00123156489"));

        // Act
        await AuthorizeAsync();
        var response = await Client.PostAsJsonAsync(UrlUser, request);
        var result = await response.Content.ReadAsJsonAsync<CreateEntityResponse>();
        
        var userResponse = await Client.GetAsync(FormatUrl(result?.Id ?? Guid.Empty));
        var userResult = await userResponse.Content.ReadAsJsonAsync<GetUserResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, userResponse.StatusCode);
        Assert.NotNull(userResult);
    }
    
    [Fact(DisplayName = "Get user - Should return not found")]
    public async Task GetAsync_ShouldReturnNotFound()
    {
        // Act
        await AuthorizeAsync();
        var userResponse = await Client.GetAsync(FormatUrl(Guid.Empty));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, userResponse.StatusCode);
    }
    
    [Fact(DisplayName = "Update user - Should return not found")]
    public async Task UpdateAsync_ShouldReturnNotFound()
    {
        // Arrange
        var request = UserMock.UpdateUserRequest();
        
        // Act
        await AuthorizeAsync();
        var userResponse = await Client.PatchAsJsonAsync(FormatUrl(Guid.Empty), request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, userResponse.StatusCode);
    }
    
    [Fact(DisplayName = "Update user - Should update an user")]
    public async Task UpdateAsync_ShouldUpdateAnUser()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest(
            "Johns Doe",
            "johnsdoe@example.com",
            "johns_doe",
            "stringadasdasd",
            new UserContactRequest("00123456759")
        );
        var updateUserRequest = UserMock.UpdateUserRequest();

        // Act
        await AuthorizeAsync();
        var response = await Client.PostAsJsonAsync(UrlUser, createUserRequest);
        var result = await response.Content.ReadAsJsonAsync<CreateEntityResponse>();
        
        var userResponse = await Client.PatchAsJsonAsync(FormatUrl(result?.Id!), updateUserRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, userResponse.StatusCode);
    }
    
    [InlineData("stringadasdasdasdasdradaidiashdjasodjasdlkasjdasjdlkajd", "john@example.com", "john", "stringadasdasd", "00123656789", "Name")]
    [InlineData("John Doe", "", "john", "stringadasdasd", "00123656789", "Email")]
    [InlineData("John Doe", "john@example.com", "", "stringadasdasd", "00123656789", "Login")]
    [InlineData("John Doe", "john@example.com", "john", "", "00123656789", "Password")]
    [InlineData("John Doe", "john@example.com", "john", "stri", "00123656789", "Password")]
    [InlineData("John Doe", "john@example.com", "john", "stringadasdasd", "", "Contact.PhoneNumber")]
    [InlineData("John Doe", "john@example.com", "john", "stringadasdasd", "00123656", "Contact.PhoneNumber")]
    [InlineData("John Doe", "john@example.com", "john", "stringadasdasd", "0012365678923423", "Contact.PhoneNumber")]
    [Theory(DisplayName = "Create user - Should return bad request when invalid request")]
    public async Task UpdateAsync_ShouldReturnBadRequest_WhenInvalidRequest(
        string name,
        string email,
        string login,
        string password,
        string phoneNumber,
        string expectedErrorMessage)
    {
        // Arrange
        var request = new UpdateUserRequest(
            name,
            email,
            login,
            password,
            new UserContactRequest(phoneNumber));

        // Act
        await AuthorizeAsync();
        var response = await Client.PatchAsJsonAsync(FormatUrl(Guid.Empty), request);
        var result = await response.Content.ReadAsJsonAsync<ExceptionResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.True(result!.Errors!.ContainsKey(expectedErrorMessage));
    }
    
    [Fact(DisplayName = "Delete user - Should return not found")]
    public async Task DeleteAsync_ShouldReturnNotFound()
    {
        // Act
        await AuthorizeAsync();
        var userResponse = await Client.DeleteAsync(FormatUrl(Guid.Empty));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, userResponse.StatusCode);
    }
    
    [Fact(DisplayName = "Delete user - Should return delete an user")]
    public async Task DeleteAsync_ShouldDeleteAnUser()
    {
        //Arrange
        var createUserRequest = new CreateUserRequest(
            "Johns Doe",
            "johns1doe@example.com",
            "johns1_doe",
            "stringadasdasd",
            new UserContactRequest("00523456759")
        );

        // Act
        await AuthorizeAsync();
        var response = await Client.PostAsJsonAsync(UrlUser, createUserRequest);
        var result = await response.Content.ReadAsJsonAsync<CreateEntityResponse>();
        
        var userResponse = await Client.DeleteAsync(FormatUrl(result?.Id!));

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, userResponse.StatusCode);
    }
    
    [Fact(DisplayName = "Get users - Should return a user list")]
    public async Task ListAsync_ShouldReturnAUserList()
    {
        // Act
        await AuthorizeAsync();
        var userResponse = await Client.GetAsync(UrlUser);
        var userResult = await userResponse.Content.ReadAsJsonAsync<GetUserResponse[]>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, userResponse.StatusCode);
        Assert.NotNull(userResult);
        Assert.NotEmpty(userResult);
    }
}