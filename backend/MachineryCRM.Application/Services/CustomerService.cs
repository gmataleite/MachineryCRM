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

        return new CustomerDto 
        { 
            Id = customer.Id, 
            Name = customer.Name, 
            IsActive = customer.IsActive 
        };
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllWithDetailsAsync();
        
        return customers.Select(c => new CustomerDto 
        { 
            Id = c.Id, 
            Name = c.Name,
            IsActive = c.IsActive,
            Sites = c.Sites?.Select(s => new SiteDto { Id = s.Id, Name = s.Name, Address = s.Address }).ToList() ?? new List<SiteDto>(),
            FiscalEntities = c.FiscalEntities?.Select(f => new FiscalEntityDto { Id = f.Id, Name = f.Name, TaxId = f.TaxId, BillingAddress = f.BillingAddress, ShippingAddress = f.ShippingAddress }).ToList() ?? new List<FiscalEntityDto>(),
            Contacts = c.Contacts?.Select(ct => new ContactDto { Id = ct.Id, Name = ct.Name, SiteId = ct.SiteId, FiscalEntityId = ct.FiscalEntityId, Phone = ct.Phone, Email = ct.Email, Observations = ct.Observations }).ToList() ?? new List<ContactDto>()
        });
    }

    public async Task<CustomerDto?> GetByIdWithDetailsAsync(Guid id)
    {
        var customer = await _customerRepository.GetCustomerWithDetailsAsync(id);
        if (customer == null) return null;

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            IsActive = customer.IsActive,
            Sites = customer.Sites?.Select(s => new SiteDto 
            { 
                Id = s.Id, 
                Name = s.Name, 
                Address = s.Address, 
                GeoPoints = s.GeoPoints?.OrderBy(g => g.Order).Select(g => new GeoPointDto { Id = g.Id, Description = g.Description, Latitude = g.Latitude, Longitude = g.Longitude, LocationType = g.LocationType, Order = g.Order }).ToList() ?? new List<GeoPointDto>()
            }).ToList() ?? new List<SiteDto>(),
            FiscalEntities = customer.FiscalEntities?.Select(f => new FiscalEntityDto { Id = f.Id, Name = f.Name, TaxId = f.TaxId, BillingAddress = f.BillingAddress, ShippingAddress = f.ShippingAddress }).ToList() ?? new List<FiscalEntityDto>(),
            Contacts = customer.Contacts?.Select(c => new ContactDto { Id = c.Id, SiteId = c.SiteId, FiscalEntityId = c.FiscalEntityId, Name = c.Name, Phone = c.Phone, Email = c.Email, Observations = c.Observations }).ToList() ?? new List<ContactDto>()
        };
    }

    public async Task<SiteDto> AddSiteAsync(Guid customerId, CreateSiteDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");

        var site = new Site(customerId, dto.Name, dto.Address);
        _customerRepository.AddSite(site);
        await _unitOfWork.CommitAsync();

        return new SiteDto { Id = site.Id, Name = site.Name, Address = site.Address };
    }

    public async Task<FiscalEntityDto> AddFiscalEntityAsync(Guid customerId, CreateFiscalEntityDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");

        var fiscal = new FiscalEntity(customerId, dto.Name, dto.TaxId, dto.BillingAddress, dto.ShippingAddress);
        _customerRepository.AddFiscalEntity(fiscal);
        await _unitOfWork.CommitAsync();

        return new FiscalEntityDto { Id = fiscal.Id, Name = fiscal.Name, TaxId = fiscal.TaxId, BillingAddress = fiscal.BillingAddress, ShippingAddress = fiscal.ShippingAddress };
    }

    public async Task<ContactDto> AddContactAsync(Guid customerId, CreateContactDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");

        var contact = new Contact(customerId, dto.Name, dto.Phone, dto.Email, dto.Observations);
        
        if (dto.SiteId.HasValue) contact.ChangeSite(dto.SiteId.Value);
        else if (dto.FiscalEntityId.HasValue) contact.ChangeFiscalEntity(dto.FiscalEntityId.Value);
        
        _customerRepository.AddContact(contact);
        await _unitOfWork.CommitAsync();

        return new ContactDto { Id = contact.Id, SiteId = contact.SiteId, FiscalEntityId = contact.FiscalEntityId, Name = contact.Name, Phone = contact.Phone, Email = contact.Email };
    }

    public async Task UpdateCustomerAsync(Guid id, UpdateCustomerDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");
        
        customer.UpdateDetails(dto.Name);

        _customerRepository.Update(customer);
        await _unitOfWork.CommitAsync();
    }

    public async Task ToggleCustomerStatusAsync(Guid id, bool activate)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) throw new KeyNotFoundException("Cliente não encontrado.");
        
        if (activate)
            customer.Activate();
        else
            customer.Deactivate();

        _customerRepository.Update(customer);   
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateSiteAsync(Guid siteId, UpdateSiteDto dto)
    {
        var site = await _customerRepository.GetSiteByIdAsync(siteId);
        if (site == null) throw new KeyNotFoundException("Local não encontrado.");
        
        site.UpdateDetails(dto.Name, dto.Address);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteSiteAsync(Guid siteId)
    {
        var site = await _customerRepository.GetSiteByIdAsync(siteId);
        if (site == null) throw new KeyNotFoundException("Local não encontrado.");
        
        _customerRepository.RemoveSite(site);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateFiscalEntityAsync(Guid fiscalId, UpdateFiscalEntityDto dto)
    {
        var fiscal = await _customerRepository.GetFiscalEntityByIdAsync(fiscalId);
        if (fiscal == null) throw new KeyNotFoundException("Ente fiscal não encontrado.");
        
        fiscal.UpdateDetails(dto.Name, dto.TaxId, dto.BillingAddress, dto.ShippingAddress);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteFiscalEntityAsync(Guid fiscalId)
    {
        var fiscal = await _customerRepository.GetFiscalEntityByIdAsync(fiscalId);
        if (fiscal == null) throw new KeyNotFoundException("Ente fiscal não encontrado.");
        
        _customerRepository.RemoveFiscalEntity(fiscal);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateContactAsync(Guid contactId, UpdateContactDto dto)
    {
        var contact = await _customerRepository.GetContactByIdAsync(contactId);
        if (contact == null) throw new KeyNotFoundException("Contato não encontrado.");
        
        // Correção aplicada: dto.Observations mapeado ao invés de null estático
        contact.UpdateDetails(dto.Name, dto.Phone, dto.Email, dto.Observations);
        
        // Regras de transferência de agregado
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

    public async Task<GeoPointDto> AddGeoPointAsync(Guid siteId, CreateGeoPointDto dto)
    {
        var site = await _customerRepository.GetSiteByIdAsync(siteId);
        if (site == null) throw new KeyNotFoundException("Local produtivo não encontrado.");

        var geoPoint = new GeoPoint(siteId, dto.Description, dto.Latitude, dto.Longitude, dto.Order, dto.LocationType);
        _customerRepository.AddGeoPoint(geoPoint);
        await _unitOfWork.CommitAsync();

        return new GeoPointDto { Id = geoPoint.Id, Description = geoPoint.Description, Latitude = geoPoint.Latitude, Longitude = geoPoint.Longitude, Order = geoPoint.Order };
    }

    public async Task UpdateGeoPointAsync(Guid geoPointId, UpdateGeoPointDto dto)
    {
        var geoPoint = await _customerRepository.GetGeoPointByIdAsync(geoPointId);
        if (geoPoint == null) throw new KeyNotFoundException("Ponto geográfico não encontrado.");

        geoPoint.UpdateDetails(dto.Description, dto.Latitude, dto.Longitude, dto.Order);
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
        var site = await _customerRepository.GetSiteByIdWithGeoPointsAsync(siteId);
        if (site == null) throw new KeyNotFoundException("Site não encontrado.");

        var orderedIds = dtos.OrderBy(dto => dto.Order).Select(dto => dto.Id).ToList();
        
        site.ReorderGeoPoints(orderedIds);
        await _unitOfWork.CommitAsync();
    }

    public async Task<SiteDto> GetSiteByIdWithGeoPointsAsync(Guid id)
    {
        var site = await _customerRepository.GetSiteByIdWithGeoPointsAsync(id);
        if (site == null) throw new KeyNotFoundException("Site não encontrado.");

        return new SiteDto
        {
            Id = site.Id,
            Name = site.Name,
            Address = site.Address,
            GeoPoints = site.GeoPoints.OrderBy(g => g.Order).Select(g => new GeoPointDto { Id = g.Id, Description = g.Description, Latitude = g.Latitude, Longitude = g.Longitude, LocationType = g.LocationType, Order = g.Order }).ToList()
        };
    }
}