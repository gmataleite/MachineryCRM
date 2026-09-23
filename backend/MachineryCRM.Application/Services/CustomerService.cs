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

// --- CUSTOMER ---
public async Task UpdateCustomerAsync(Guid id, UpdateCustomerDto dto)
{
    var customer = await _customerRepository.GetByIdAsync(id);
    if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");
    customer.UpdateName(dto.Name);
    _customerRepository.Update(customer);
    await _unitOfWork.CommitAsync();
}

public async Task DeleteCustomerAsync(Guid id)
{
    var customer = await _customerRepository.GetByIdAsync(id);
    if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");
    _customerRepository.Remove(customer); // A exclusão em cascata deve cuidar dos filhos se configurada no EF
    await _unitOfWork.CommitAsync();
}

// --- SITE ---
public async Task UpdateSiteAsync(Guid siteId, UpdateSiteDto dto)
{
    var site = await _customerRepository.GetSiteByIdAsync(siteId);
    if (site == null) throw new KeyNotFoundException("Local não encontrado.");
    site.UpdateDetails(dto.Name, dto.Country, dto.State, dto.City);
    await _unitOfWork.CommitAsync(); // O EF rastreia a mudança da entidade carregada
}

public async Task DeleteSiteAsync(Guid siteId)
{
    var site = await _customerRepository.GetSiteByIdAsync(siteId);
    if (site == null) throw new KeyNotFoundException("Local não encontrado.");
    _customerRepository.RemoveSite(site);
    await _unitOfWork.CommitAsync();
}

// --- FISCAL ENTITY ---
public async Task UpdateFiscalEntityAsync(Guid fiscalId, UpdateFiscalEntityDto dto)
{
    var fiscal = await _customerRepository.GetFiscalEntityByIdAsync(fiscalId);
    if (fiscal == null) throw new KeyNotFoundException("Ente fiscal não encontrado.");
    fiscal.UpdateDetails(dto.Name, dto.SapPn, dto.Cnpj, dto.Cpf, dto.Country, dto.State, dto.City);
    await _unitOfWork.CommitAsync();
}

public async Task DeleteFiscalEntityAsync(Guid fiscalId)
{
    var fiscal = await _customerRepository.GetFiscalEntityByIdAsync(fiscalId);
    if (fiscal == null) throw new KeyNotFoundException("Ente fiscal não encontrado.");
    _customerRepository.RemoveFiscalEntity(fiscal);
    await _unitOfWork.CommitAsync();
}

// --- CONTACT ---
public async Task UpdateContactAsync(Guid contactId, UpdateContactDto dto)
{
    var contact = await _customerRepository.GetContactByIdAsync(contactId);
    if (contact == null) throw new KeyNotFoundException("Contato não encontrado.");
    
    contact.UpdateDetails(dto.Description, dto.Phone, dto.Email, null);
    
    // Lógica para permitir arrastar/mover contatos entre cards no frontend
    if (contact.SiteId != dto.SiteId) contact.ChangeSite(dto.SiteId);
    if (contact.FiscalEntityId != dto.FiscalEntityId) contact.ChangeFiscalEntity(dto.FiscalEntityId);

    await _unitOfWork.CommitAsync();
}

public async Task DeleteContactAsync(Guid contactId)
{
    var contact = await _customerRepository.GetContactByIdAsync(contactId);
    if (contact == null) throw new KeyNotFoundException("Contato não encontrado.");
    _customerRepository.RemoveContact(contact);
    await _unitOfWork.CommitAsync();
}
}