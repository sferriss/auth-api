using Auth.Api.Mappers;
using Auth.Api.Models.Requests;
using Auth.Api.Models.Responses;
using Auth.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("user")]
public class UserController(IUserService userService, UserMapper userMapper) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateEntityResponse))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
    {
        var result = await userService.CreateAsync(userMapper.ToDto(request));
        return Created(uri: string.Empty, new CreateEntityResponse(result));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserResponse))]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var result = await userService.GetAsync(id);
        return Ok(userMapper.ToResponse(result));
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
    {
        await userService.UpdateAsync(id, userMapper.ToDto(request));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await userService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserResponse[]))]
    public async Task<IActionResult> GetAsync()
    {
        var result = await userService.GetAsync();
        return Ok(result.Select(userMapper.ToResponse));
    }
}