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
}