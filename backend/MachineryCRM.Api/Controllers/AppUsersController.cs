using Asp.Versioning;
using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MachineryCRM.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "Admin,Manager")]
public class AppUsersController : ControllerBase
{
    private readonly IAppUserService _appUserService;

    public AppUsersController(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppUserDto dto)
    {
        try
        {
            var result = await _appUserService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("{email}/reset-password")]
    public async Task<IActionResult> ResetPassword(string email)
    {
        try
        {
            var result = await _appUserService.ResetPasswordAsync(email);
            return Ok(new { result.User, result.TemporaryPassword });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found." });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _appUserService.GetByIdAsync(id);
        if (user == null) return NotFound(new { message = "User not found." });

        return Ok(user);
    }
}
