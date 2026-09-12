using MachineryCRM.Domain.Entities;

namespace MachineryCRM.Domain.Interfaces;

public interface IMachineRepository : IRepository<Machine>
{
    Task<Machine?> GetBySerialNumberAsync(string serialNumber);
}