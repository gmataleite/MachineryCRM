using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Interfaces;
using MachineryCRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MachineryCRM.Infrastructure.Repositories;

public class MachineRepository : Repository<Machine>, IMachineRepository
{
    public MachineRepository(AppDbContext context) : base(context) { }

    public async Task<Machine?> GetBySerialNumberAsync(string serialNumber)
    {
        return await _context.Machines.FirstOrDefaultAsync(m => m.SerialNumber == serialNumber);
    }
}