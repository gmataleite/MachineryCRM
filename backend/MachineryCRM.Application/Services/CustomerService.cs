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
        Sites = c.Sites.Select(s => new SiteDto { Id = s.Id, Name = s.Name, City = s.City, State = s.State, CountryCode = s.CountryCode }).ToList(),
        FiscalEntities = c.FiscalEntities.Select(f => new FiscalEntityDto { Id = f.Id, Name = f.Name, TaxId = f.TaxId, BillingAddressLine = f.BillingAddressLine, BillingNeighborhood = f.BillingNeighborhood, BillingCity = f.BillingCity, BillingState = f.BillingState, BillingPostalCode = f.BillingPostalCode, BillingCountryCode = f.BillingCountryCode, ShippingAddressLine = f.ShippingAddressLine, ShippingNeighborhood = f.ShippingNeighborhood, ShippingCity = f.ShippingCity, ShippingState = f.ShippingState, ShippingPostalCode = f.ShippingPostalCode, ShippingCountryCode = f.ShippingCountryCode }).ToList(),
        Contacts = c.Contacts.Select(ct => new ContactDto { Id = ct.Id, Name = ct.Name }).ToList()
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
            Id = s.Id, Name = s.Name, City = s.City, State = s.State, CountryCode = s.CountryCode, Observations = s.Observations, GeoPoints = s.GeoPoints.OrderBy(g => g.Order).Select(g => new GeoPointDto
            {
                Id = g.Id,
                Description = g.Description,
                Latitude = g.Latitude,
                Longitude = g.Longitude,
                LocationType = g.LocationType,
                Order = g.Order }).ToList()
        }).ToList(),
        FiscalEntities = customer.FiscalEntities.Select(f => new FiscalEntityDto 
        { 
            Id = f.Id, Name = f.Name, TaxId = f.TaxId, BillingAddressLine = f.BillingAddressLine, BillingNeighborhood = f.BillingNeighborhood, BillingCity = f.BillingCity, BillingState = f.BillingState, BillingPostalCode = f.BillingPostalCode, BillingCountryCode = f.BillingCountryCode, ShippingAddressLine = f.ShippingAddressLine, ShippingNeighborhood = f.ShippingNeighborhood, ShippingCity = f.ShippingCity, ShippingState = f.ShippingState, ShippingPostalCode = f.ShippingPostalCode, ShippingCountryCode = f.ShippingCountryCode 
        }).ToList(),
        Contacts = customer.Contacts.Select(c => new ContactDto 
        { 
            Id = c.Id, SiteId = c.SiteId, FiscalEntityId = c.FiscalEntityId, Name = c.Name, Phone = c.Phone, Email = c.Email 
        }).ToList()
    };

    return customerDto;
}

public async Task<SiteDto> AddSiteAsync(Guid customerId, CreateSiteDto dto)
{
    // Busca leve: carrega apenas o cliente, sem as listas pesadas
    var customer = await _customerRepository.GetByIdAsync(customerId);
    if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");

    var site = new Site(customerId, dto.Name, dto.City, dto.State, dto.CountryCode, dto.Observations);
    
    // Inserção direta sem modificar a entidade Customer
    _customerRepository.AddSite(site);
    await _unitOfWork.CommitAsync();

    return new SiteDto 
    { 
        Id = site.Id, Name = site.Name, City = site.City, State = site.State, CountryCode = site.CountryCode, Observations = site.Observations 
    };
}

public async Task<FiscalEntityDto> AddFiscalEntityAsync(Guid customerId, CreateFiscalEntityDto dto)
{
    var customer = await _customerRepository.GetByIdAsync(customerId);
    if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");

    var fiscal = new FiscalEntity(customerId, dto.Name, dto.TaxId, dto.BillingAddressLine, dto.BillingNeighborhood, dto.BillingCity, dto.BillingState, dto.BillingPostalCode, dto.BillingCountryCode, dto.ShippingAddressLine, dto.ShippingNeighborhood, dto.ShippingCity, dto.ShippingState, dto.ShippingPostalCode, dto.ShippingCountryCode);
    
    // Inserção direta sem modificar a entidade Customer
    _customerRepository.AddFiscalEntity(fiscal);
    await _unitOfWork.CommitAsync();

    return new FiscalEntityDto 
    { 
        Id = fiscal.Id, Name = fiscal.Name, TaxId = fiscal.TaxId, BillingAddressLine = fiscal.BillingAddressLine, BillingNeighborhood = fiscal.BillingNeighborhood, BillingCity = fiscal.BillingCity, BillingState = fiscal.BillingState, BillingPostalCode = fiscal.BillingPostalCode, BillingCountryCode = fiscal.BillingCountryCode, ShippingAddressLine = fiscal.ShippingAddressLine, ShippingNeighborhood = fiscal.ShippingNeighborhood, ShippingCity = fiscal.ShippingCity, ShippingState = fiscal.ShippingState, ShippingPostalCode = fiscal.ShippingPostalCode, ShippingCountryCode = fiscal.ShippingCountryCode 
    };
}

public async Task<ContactDto> AddContactAsync(Guid customerId, CreateContactDto dto)
{
    var customer = await _customerRepository.GetByIdAsync(customerId);
    if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");

    var contact = new Contact(customerId, dto.Name, dto.Phone, dto.Email);
    
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
        Name = contact.Name, 
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
    site.UpdateDetails(dto.Name, dto.City, dto.State, dto.CountryCode, dto.Observations);
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
    fiscal.UpdateDetails(dto.Name, dto.TaxId, dto.BillingAddressLine, dto.BillingNeighborhood, dto.BillingCity, dto.BillingState, dto.BillingPostalCode, dto.BillingCountryCode, dto.ShippingAddressLine, dto.ShippingNeighborhood, dto.ShippingCity, dto.ShippingState, dto.ShippingPostalCode, dto.ShippingCountryCode);
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
    
    contact.UpdateDetails(dto.Name, dto.Phone, dto.Email, null);
    
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

// --- GEO POINT ---
public async Task<GeoPointDto> AddGeoPointAsync(Guid siteId, CreateGeoPointDto dto)
{
    var site = await _customerRepository.GetSiteByIdAsync(siteId);
    if (site == null) throw new KeyNotFoundException("Local produtivo não encontrado.");

    var geoPoint = new GeoPoint(siteId, dto.Description, dto.Latitude, dto.Longitude, dto.LocationType, dto.Order);
    _customerRepository.AddGeoPoint(geoPoint);
    await _unitOfWork.CommitAsync();

    return new GeoPointDto { Id = geoPoint.Id, Description = geoPoint.Description, Latitude = geoPoint.Latitude, Longitude = geoPoint.Longitude, Order = geoPoint.Order };
}

public async Task UpdateGeoPointAsync(Guid geoPointId, UpdateGeoPointDto dto)
{
    var geoPoint = await _customerRepository.GetGeoPointByIdAsync(geoPointId);
    if (geoPoint == null) throw new KeyNotFoundException("Ponto geográfico não encontrado.");

    geoPoint.UpdateDetails(dto.Description, dto.Latitude, dto.Longitude);
    await _unitOfWork.CommitAsync();
}

public async Task DeleteGeoPointAsync(Guid geoPointId)
{
    var geoPoint = await _customerRepository.GetGeoPointByIdAsync(geoPointId);
    if (geoPoint == null) throw new KeyNotFoundException("Ponto geográfico não encontrado.");

    _customerRepository.RemoveGeoPoint(geoPoint);
    await _unitOfWork.CommitAsync();
}

public async Task ReorderGeoPointsAsync(Guid siteId, List<ReorderGeoPointDto> dtos)
{
    var geoPoints = await _customerRepository.GetGeoPointsBySiteIdAsync(siteId);
    
    foreach (var dto in dtos)
    {
        var point = geoPoints.FirstOrDefault(g => g.Id == dto.Id);
        if (point != null) point.SetOrder(dto.Order);
    }

    await _unitOfWork.CommitAsync();
}
}