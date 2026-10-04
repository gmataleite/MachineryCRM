using MachineryCRM.Application.DTOs;

namespace MachineryCRM.Application.Interfaces;

public interface IMaintenanceService
{
    Task<MaintenanceDto> CreateAsync(CreateMaintenanceDto dto);
    Task StartMaintenanceAsync(Guid machineId, Guid maintenanceId);
    Task CompleteMaintenanceAsync(Guid machineId, Guid maintenanceId);
}