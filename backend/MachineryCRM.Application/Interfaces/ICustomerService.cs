using MachineryCRM.Application.DTOs;

namespace MachineryCRM.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDto?> GetByIdWithDetailsAsync(Guid id);
    Task<IEnumerable<CustomerDto>> GetAllAsync();
}