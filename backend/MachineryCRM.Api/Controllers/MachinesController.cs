using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace MachineryCRM.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")] // Versioned API routes
[Authorize] // Enforces JWT authentication globally for this controller
public class MachinesController : ControllerBase
{
    private readonly IMachineService _machineService;

    public MachinesController(IMachineService machineService)
    {
        _machineService = machineService;
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")] // Strict RBAC enforcement for writes
    public async Task<IActionResult> Create([FromBody] CreateMachineDto dto)
    {
        try
        {
            var result = await _machineService.CreateAsync(dto);
            // Returns 201 Created with the Location header pointing to the GET endpoint
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            // Returns 409 Conflict if the business rule (duplicate serial) fails
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Manager,Admin,Technician")] // Broad RBAC for reads
    public async Task<IActionResult> GetAll()
    {
        var result = await _machineService.GetAllAsync();
        return Ok(result); // Returns 200 OK
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Manager,Admin,Technician")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _machineService.GetByIdAsync(id);
        
        if (result == null)
            return NotFound(); // Returns 404 Not Found
        
        return Ok(result);
    }
}
