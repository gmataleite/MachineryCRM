using MachineryCRM.Domain.Enums;

namespace MachineryCRM.Application.DTOs;

public class MaintenanceDto
{
    public Guid Id { get; set; }
    public Guid MachineId { get; set; }
    public Guid AppUserId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string MaintenanceType { get; set; } = null!;
    public string? PartsUsed { get; set; }
    public MaintenanceStatus Status { get; set; }
    public DateTime? CompletionDate { get; set; }
}

public class CreateMaintenanceDto
{
    public Guid MachineId { get; set; }
    public Guid AppUserId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string MaintenanceType { get; set; } = null!;
    public string? PartsUsed { get; set; }
}