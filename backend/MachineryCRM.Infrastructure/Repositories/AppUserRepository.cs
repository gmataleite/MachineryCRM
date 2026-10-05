using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Interfaces;
using MachineryCRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MachineryCRM.Infrastructure.Repositories;

public class AppUserRepository : Repository<AppUser>, IAppUserRepository
{
    public AppUserRepository(AppDbContext context) : base(context) { }

    public async Task<AppUser?> GetByEmailAsync(string email)
    {
        return await _context.Set<AppUser>().FirstOrDefaultAsync(u => u.Email == email);
    }
}