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
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly ICreateAdminUserService _createAdminUserService;
    private readonly UserMapper _userMapper;

    public LoginController(ILoginService loginService, UserMapper userMapper, ICreateAdminUserService createAdminUserService)
    {
        _loginService = loginService;
        _userMapper = userMapper;
        _createAdminUserService = createAdminUserService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var token = await _loginService.LoginAsync(_userMapper.ToDto(request));
        return Ok(new LoginResponse(token));
    }
    
    [HttpPost("admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateAdminUserAsync()
    {
        await _createAdminUserService.CreateUserAdminAsync();
        return NoContent();
    }
}