using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Interfaces;
using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Interfaces;

namespace MachineryCRM.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var customer = new Customer(dto.Name);
        
        await _customerRepository.AddAsync(customer);
        await _unitOfWork.CommitAsync();

        return new CustomerDto { Id = customer.Id, Name = customer.Name };
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(c => new CustomerDto 
        { 
            Id = c.Id, 
            Name = c.Name 
        });
    }

    public async Task<CustomerDto?> GetByIdWithDetailsAsync(Guid id)
    {
        var customer = await _customerRepository.GetCustomerWithDetailsAsync(id);
        if (customer == null) return null;

        var customerDto = new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Sites = customer.Sites.Select(s => new SiteDto 
            { 
                Id = s.Id, Name = s.Name, Country = s.Country, State = s.State, City = s.City, Observations = s.Observations 
            }).ToList(),
            FiscalEntities = customer.FiscalEntities.Select(f => new FiscalEntityDto 
            { 
                Id = f.Id, Name = f.Name, SapPn = f.SapPn, Cnpj = f.Cnpj, Cpf = f.Cpf, Country = f.Country, State = f.State, City = f.City 
            }).ToList(),
            Contacts = customer.Contacts.Select(c => new ContactDto 
            { 
                Id = c.Id, SiteId = c.SiteId, FiscalEntityId = c.FiscalEntityId, Description = c.Description, Phone = c.Phone, Email = c.Email 
            }).ToList()
        };

        return customerDto;
    }
}