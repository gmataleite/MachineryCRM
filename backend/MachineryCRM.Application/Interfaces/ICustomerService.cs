using MachineryCRM.Application.DTOs;

namespace MachineryCRM.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDto?> GetByIdWithDetailsAsync(Guid id);
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<SiteDto> AddSiteAsync(Guid customerId, CreateSiteDto dto);
    Task<FiscalEntityDto> AddFiscalEntityAsync(Guid customerId, CreateFiscalEntityDto dto);
    Task<ContactDto> AddContactAsync(Guid customerId, CreateContactDto dto);
}