﻿using System.Net;
using System.Net.Http.Json;
using Auth.Api.ExceptionHandlers.Responses;
using Auth.Api.Models.Requests;
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
    
    [Fact(DisplayName = "Create user - Should create new user")]
    public async Task CreateAsync_ShouldCreateNewUser()
    {
        // Arrange
        var request = UserMock.CreateUserRequest();

        // Act
        await AuthorizeAsync();
        var response = await Client.PostAsJsonAsync(Url, request);
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
        var response = await Client.PostAsJsonAsync(Url, request);

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
        await Client.PostAsJsonAsync(Url, request);
        var response2 = await Client.PostAsJsonAsync(Url, request2);
        var result = await response2.Content.ReadAsJsonAsync<ExceptionResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
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
        await Client.PostAsJsonAsync(Url, request);
        var response2 = await Client.PostAsJsonAsync(Url, request2);
        var result = await response2.Content.ReadAsJsonAsync<ExceptionResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
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
        await Client.PostAsJsonAsync(Url, request);
        var response2 = await Client.PostAsJsonAsync(Url, request2);
        var result = await response2.Content.ReadAsJsonAsync<ExceptionResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
        Assert.Equal("Phone number already exists", result?.Title);
    }
}