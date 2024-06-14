using Auth.Api.Mappers;
using Auth.Api.Models.Requests;
using Auth.Api.Models.Responses;
using Auth.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserMapper _userMapper;

    public UserController(IUserService userService, UserMapper userMapper)
    {
        _userService = userService;
        _userMapper = userMapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateEntityResponse))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateAsync(_userMapper.ToDto(request));
        return Created(uri: string.Empty, new CreateEntityResponse(result));
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserResponse))]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var result = await _userService.GetAsync(id);
        return Ok(_userMapper.ToResponse(result));
    }
    
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
    {
        await _userService.UpdateAsync(id, _userMapper.ToDto(request));
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserResponse[]))]
    public async Task<IActionResult> GetAsync()
    {
        var result = await _userService.GetAsync();
        return Ok(result.Select(_userMapper.ToResponse));
    }
}