using MachineryCRM.Domain.Entities;

namespace MachineryCRM.Domain.Interfaces;

public interface IMachineRepository : IRepository<Machine>
{
    Task<Machine?> GetBySerialNumberAsync(string serialNumber);
    Task<Machine?> GetMachineWithDetailsAsync(Guid id);
    
    void AddMaintenance(Maintenance maintenance);
    void AddTransferHistory(TransferHistory transfer);
    void AddCommunication(Communication communication);
}