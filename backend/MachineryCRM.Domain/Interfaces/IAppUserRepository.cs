using MachineryCRM.Domain.Entities;

namespace MachineryCRM.Domain.Interfaces;

public interface IAppUserRepository : IRepository<AppUser>
{
    Task<AppUser?> GetByEmailAsync(string email);
}