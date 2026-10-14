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

    // --- CRUD ---

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _appUserService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _appUserService.GetByIdAsync(id);
        if (user == null) return NotFound(new { message = "User not found." });

        return Ok(user);
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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppUserDto dto)
    {
        try
        {
            await _appUserService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        try
        {
            await _appUserService.DeactivateAsync(id);
            return NoContent(); 
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found." });
        }
    }

    // --- AUTENTICAÇÃO / ACESSO ---

    [HttpPost("{id:guid}/reset-password")]
    public async Task<IActionResult> ResetPassword(Guid id)
    {
        try
        {
            var result = await _appUserService.ResetPasswordByIdAsync(id);
            return Ok(new { temporaryPassword = result.TemporaryPassword }); 
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found." });
        }
    }

    [HttpPatch("{id:guid}/reactivate")]
    public async Task<IActionResult> ReactivateUser(Guid id)
    {
        try
        {
            var result = await _appUserService.ReactivateAndResetPasswordAsync(id);
            return Ok(new { temporaryPassword = result.TemporaryPassword }); 
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "User not found." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}