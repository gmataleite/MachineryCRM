using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Interfaces;
using MachineryCRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MachineryCRM.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }

    public async Task<Customer?> GetCustomerWithDetailsAsync(Guid id)
    {
        return await _context.Customers
            .Include(c => c.Sites)
                .ThenInclude(s => s.GeoPoints)
            .Include(c => c.FiscalEntities)
            .Include(c => c.Contacts)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
    {
        return await _context.Customers
            .Include(c => c.Sites)
            .Include(c => c.FiscalEntities)
            .Include(c => c.Contacts)
            .ToListAsync();
    }

    public async Task<Site?> GetSiteByIdAsync(Guid id) => await _context.Sites.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<FiscalEntity?> GetFiscalEntityByIdAsync(Guid id) => await _context.FiscalEntities.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Contact?> GetContactByIdAsync(Guid id) => await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);

    public void AddSite(Site site) => _context.Sites.Add(site);

    public void AddFiscalEntity(FiscalEntity fiscalEntity) => _context.FiscalEntities.Add(fiscalEntity);

    public void Remove(Customer customer) => _context.Customers.Remove(customer);
    
    public void RemoveSite(Site site) => _context.Sites.Remove(site);

    public void RemoveFiscalEntity(FiscalEntity fiscalEntity) => _context.FiscalEntities.Remove(fiscalEntity);

    public void AddContact(Contact contact) => _context.Contacts.Add(contact);

    public void RemoveContact(Contact contact) => _context.Contacts.Remove(contact);

}