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

    public async Task<Machine?> GetMachineWithDetailsAsync(Guid id)
    {
        return await _context.Machines
            .AsSplitQuery()
            .Include(m => m.TransferHistories)
            .Include(m => m.Maintenances)
            .Include(m => m.Communications)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
    
    public void AddMaintenance(Maintenance maintenance) => _context.Set<Maintenance>().Add(maintenance);
    public void AddTransferHistory(TransferHistory transfer) => _context.Set<TransferHistory>().Add(transfer);
    public void AddCommunication(Communication communication) => _context.Set<Communication>().Add(communication);
}