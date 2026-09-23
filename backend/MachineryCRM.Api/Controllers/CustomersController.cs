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

    [HttpPost("{id:guid}/sites")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> AddSite(Guid id, [FromBody] CreateSiteDto dto)
    {
        try
        {
            var result = await _customerService.AddSiteAsync(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{id:guid}/fiscal-entities")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> AddFiscalEntity(Guid id, [FromBody] CreateFiscalEntityDto dto)
    {
        try
        {
            var result = await _customerService.AddFiscalEntityAsync(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{id:guid}/contacts")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> AddContact(Guid id, [FromBody] CreateContactDto dto)
    {
        try
        {
            var result = await _customerService.AddContactAsync(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    // ---- CUSTOMERS CRUD ----
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerDto dto)
    {
        await _customerService.UpdateCustomerAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        await _customerService.DeleteCustomerAsync(id);
        return NoContent();
    }

    // ---- SITES CRUD ----
    [HttpPut("sites/{siteId:guid}")]
    public async Task<IActionResult> UpdateSite(Guid siteId, [FromBody] UpdateSiteDto dto)
    {
        await _customerService.UpdateSiteAsync(siteId, dto);
        return NoContent();
    }

    [HttpDelete("sites/{siteId:guid}")]
    public async Task<IActionResult> DeleteSite(Guid siteId)
    {
        await _customerService.DeleteSiteAsync(siteId);
        return NoContent();
    }

    // ---- FISCAL ENTITIES CRUD ----
    [HttpPut("fiscal-entities/{fiscalId:guid}")]
    public async Task<IActionResult> UpdateFiscalEntity(Guid fiscalId, [FromBody] UpdateFiscalEntityDto dto)
    {
        await _customerService.UpdateFiscalEntityAsync(fiscalId, dto);
        return NoContent();
    }

    [HttpDelete("fiscal-entities/{fiscalId:guid}")]
    public async Task<IActionResult> DeleteFiscalEntity(Guid fiscalId)
    {
        await _customerService.DeleteFiscalEntityAsync(fiscalId);
        return NoContent();
    }

    // ---- CONTACTS CRUD ----
    [HttpPut("contacts/{contactId:guid}")]
    public async Task<IActionResult> UpdateContact(Guid contactId, [FromBody] UpdateContactDto dto)
    {
        await _customerService.UpdateContactAsync(contactId, dto);
        return NoContent();
    }

    [HttpDelete("contacts/{contactId:guid}")]
    public async Task<IActionResult> DeleteContact(Guid contactId)
    {
        await _customerService.DeleteContactAsync(contactId);
        return NoContent();
    }
}