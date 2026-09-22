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

    public void AddSite(Site site)
    {
        _context.Sites.Add(site);
    }

    public void AddFiscalEntity(FiscalEntity fiscalEntity)
    {
        _context.FiscalEntities.Add(fiscalEntity);
    }

    public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
    {
        return await _context.Customers
            .Include(c => c.Sites)
            .Include(c => c.FiscalEntities)
            .Include(c => c.Contacts)
            .ToListAsync();
    }

    public void AddContact(Contact contact)
    {
        _context.Add(contact);
    }
}