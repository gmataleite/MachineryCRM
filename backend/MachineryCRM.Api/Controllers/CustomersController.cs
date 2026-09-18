using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace MachineryCRM.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize] // Enforces JWT authentication globally for this controller
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Admin")] // Restringe a criação a perfis administrativos
    public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
    {
        var result = await _customerService.CreateAsync(dto);
        // Retorna HTTP 201 Created com o cabeçalho Location apontando para a rota de GET
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _customerService.GetByIdWithDetailsAsync(id);
        
        if (customer == null) 
            return NotFound(new { message = "Customer not found." });
        
        return Ok(customer);
    }
}