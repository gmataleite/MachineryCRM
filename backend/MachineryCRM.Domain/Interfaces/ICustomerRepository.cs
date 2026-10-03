using MachineryCRM.Domain.Entities;

namespace MachineryCRM.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetCustomerWithDetailsAsync(Guid id);
    Task<IEnumerable<Customer>> GetAllWithDetailsAsync();

    Task<Site?> GetSiteByIdAsync(Guid id);
    Task<FiscalEntity?> GetFiscalEntityByIdAsync(Guid id);
    Task<Contact?> GetContactByIdAsync(Guid id);
    Task<GeoPoint?> GetGeoPointByIdAsync(Guid id);
    
    // ADICIONADO: Método que implementamos para carregar o Site e sua coleção de GeoPoints
    Task<Site?> GetSiteByIdWithGeoPointsAsync(Guid id);
    
    // REMOVIDO: Task<List<GeoPoint>> GetGeoPointsBySiteIdAsync(Guid siteId);
    // Motivo: O repositório não deve expor a busca de filhos isolados contornando o Aggregate Root (Site).

    void AddSite(Site site);
    void AddFiscalEntity(FiscalEntity fiscalEntity);
    void AddContact(Contact contact);
    void Remove(Customer customer); 
    void RemoveSite(Site site);
    void RemoveFiscalEntity(FiscalEntity fiscalEntity);
    void RemoveContact(Contact contact);
    void AddGeoPoint(GeoPoint geoPoint);
    void RemoveGeoPoint(GeoPoint geoPoint);
}