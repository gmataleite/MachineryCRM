using MachineryCRM.Domain.Enums;

namespace MachineryCRM.Domain.Entities;

public class Maintenance : Entity
{
    public Guid MachineId { get; private set; }
    public Guid AppUserId { get; private set; }
    public DateTime MaintenanceDate { get; private set; }
    public string MaintenanceType { get; private set; } = null!;
    public string? PartsUsed { get; private set; }
    public MaintenanceStatus Status { get; private set; }
    public DateTime? CompletionDate { get; private set; }


    public Maintenance(Guid machineId, Guid appUserId, DateTime maintenanceDate, string maintenanceType, string? partsUsed)
    {
        MachineId = machineId;
        AppUserId = appUserId;
        MaintenanceDate = maintenanceDate;
        MaintenanceType = maintenanceType;
        PartsUsed = partsUsed;
        Status = MaintenanceStatus.Pending;
    }

    public void StartMaintenance()
    {
        if (Status != MaintenanceStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be started.");
        
        Status = MaintenanceStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CompleteMaintenance()
    {
        if (Status != MaintenanceStatus.InProgress)
            throw new InvalidOperationException("Only in-progress orders can be completed.");
        
        Status = MaintenanceStatus.Completed;
        CompletionDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }    
}