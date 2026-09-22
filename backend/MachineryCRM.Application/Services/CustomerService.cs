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
    var customers = await _customerRepository.GetAllWithDetailsAsync();
    
    return customers.Select(c => new CustomerDto 
    { 
        Id = c.Id, 
        Name = c.Name,
        // O mapeamento das listas é obrigatório para que o frontend consiga calcular o ".length"
        Sites = c.Sites.Select(s => new SiteDto { Id = s.Id, Name = s.Name, Country = s.Country, State = s.State, City = s.City }).ToList(),
        FiscalEntities = c.FiscalEntities.Select(f => new FiscalEntityDto { Id = f.Id, Name = f.Name, Country = f.Country, State = f.State, City = f.City }).ToList(),
        Contacts = c.Contacts.Select(ct => new ContactDto { Id = ct.Id, Description = ct.Description }).ToList()
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
    
    public async Task<SiteDto> AddSiteAsync(Guid customerId, CreateSiteDto dto)
    {
        // Busca leve: carrega apenas o cliente, sem as listas pesadas
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");

        var site = new Site(customerId, dto.Name, dto.Country, dto.State, dto.City);
        
        // Inserção direta sem modificar a entidade Customer
        _customerRepository.AddSite(site);
        await _unitOfWork.CommitAsync();

        return new SiteDto 
        { 
            Id = site.Id, Name = site.Name, Country = site.Country, State = site.State, City = site.City 
        };
    }

    public async Task<FiscalEntityDto> AddFiscalEntityAsync(Guid customerId, CreateFiscalEntityDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");

        var fiscal = new FiscalEntity(customerId, dto.Name, dto.Country, dto.State, dto.City);
        
        // Inserção direta sem modificar a entidade Customer
        _customerRepository.AddFiscalEntity(fiscal);
        await _unitOfWork.CommitAsync();

        return new FiscalEntityDto 
        { 
            Id = fiscal.Id, Name = fiscal.Name, Country = fiscal.Country, State = fiscal.State, City = fiscal.City 
        };
    }

    public async Task<ContactDto> AddContactAsync(Guid customerId, CreateContactDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");

        var contact = new Contact(customerId, dto.Description, dto.Phone, dto.Email);
        
        // Se a entidade utilizar métodos para atribuir chaves estrangeiras opcionais:
        if (dto.SiteId.HasValue) contact.ChangeSite(dto.SiteId.Value);
        if (dto.FiscalEntityId.HasValue) contact.ChangeFiscalEntity(dto.FiscalEntityId.Value);
        
        _customerRepository.AddContact(contact);
        await _unitOfWork.CommitAsync();

        return new ContactDto 
        { 
            Id = contact.Id, 
            SiteId = contact.SiteId, 
            FiscalEntityId = contact.FiscalEntityId, 
            Description = contact.Description, 
            Phone = contact.Phone, 
            Email = contact.Email 
        };
    }
}