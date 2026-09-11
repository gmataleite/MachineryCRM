using MachineryCRM.Domain.Enums;

namespace MachineryCRM.Domain.Entities;

public class MaintenanceOrder : Entity
{
    public Guid MachineId { get; private set; }
    public Guid TechnicianId { get; private set; }
    public MaintenanceStatus Status { get; private set; }
    public string IssueDescription { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public DateTime? CompletionDate { get; private set; }

    public MaintenanceOrder(Guid machineId, Guid technicianId, string issueDescription, DateTime scheduledDate)
    {
        MachineId = machineId;
        TechnicianId = technicianId;
        IssueDescription = issueDescription;
        ScheduledDate = scheduledDate;
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