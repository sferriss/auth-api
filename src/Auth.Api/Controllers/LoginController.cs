using Auth.Api.Mappers;
using Auth.Api.Models.Requests;
using Auth.Api.Models.Responses;
using Auth.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("login")]
[AllowAnonymous]
public class LoginController(
    ILoginService loginService,
    UserMapper userMapper,
    ICreateAdminUserService createAdminUserService)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var token = await loginService.LoginAsync(userMapper.ToDto(request));
        return Ok(new LoginResponse(token));
    }

    [HttpPost("admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateAdminUserAsync()
    {
        await createAdminUserService.CreateUserAdminAsync();
        return NoContent();
    }
}