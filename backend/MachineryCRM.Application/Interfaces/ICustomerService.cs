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

    Task UpdateCustomerAsync(Guid id, UpdateCustomerDto dto);
    Task DeleteCustomerAsync(Guid id);

    Task UpdateSiteAsync(Guid siteId, UpdateSiteDto dto);
    Task DeleteSiteAsync(Guid siteId);

    Task UpdateFiscalEntityAsync(Guid fiscalId, UpdateFiscalEntityDto dto);
    Task DeleteFiscalEntityAsync(Guid fiscalId);

    Task UpdateContactAsync(Guid contactId, UpdateContactDto dto);
    Task DeleteContactAsync(Guid contactId);

    Task<GeoPointDto> AddGeoPointAsync(Guid siteId, CreateGeoPointDto dto);
    Task UpdateGeoPointAsync(Guid geoPointId, UpdateGeoPointDto dto);
    Task DeleteGeoPointAsync(Guid geoPointId);
    Task ReorderGeoPointsAsync(Guid siteId, List<ReorderGeoPointDto> dtos);
}