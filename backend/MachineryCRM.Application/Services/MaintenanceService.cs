using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Interfaces;
using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Enums;
using MachineryCRM.Domain.Interfaces;

namespace MachineryCRM.Application.Services;

public class MaintenanceService : IMaintenanceService
{
    private readonly IMachineRepository _machineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MaintenanceService(IMachineRepository machineRepository, IUnitOfWork unitOfWork)
    {
        _machineRepository = machineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MaintenanceDto> CreateAsync(CreateMaintenanceDto dto)
    {
        var machine = await _machineRepository.GetByIdAsync(dto.MachineId);
        if (machine == null)
            throw new KeyNotFoundException("Machine not found.");

        var maintenance = new Maintenance(dto.MachineId, dto.AppUserId, dto.MaintenanceDate, dto.MaintenanceType, dto.PartsUsed);
        
        _machineRepository.AddMaintenance(maintenance);
        await _unitOfWork.CommitAsync();

        return new MaintenanceDto
        {
            Id = maintenance.Id,
            MachineId = maintenance.MachineId,
            AppUserId = maintenance.AppUserId,
            MaintenanceDate = maintenance.MaintenanceDate,
            MaintenanceType = maintenance.MaintenanceType,
            PartsUsed = maintenance.PartsUsed,
            Status = maintenance.Status,
            CompletionDate = maintenance.CompletionDate
        };
    }

    public async Task StartMaintenanceAsync(Guid machineId, Guid maintenanceId)
    {
        var machine = await _machineRepository.GetMachineWithDetailsAsync(machineId);
        if (machine == null) throw new KeyNotFoundException("Machine not found.");

        var maintenance = machine.Maintenances.FirstOrDefault(m => m.Id == maintenanceId);
        if (maintenance == null) throw new KeyNotFoundException("Maintenance not found on this machine.");

        maintenance.StartMaintenance();
        await _unitOfWork.CommitAsync();
    }

    public async Task CompleteMaintenanceAsync(Guid machineId, Guid maintenanceId)
    {
        var machine = await _machineRepository.GetMachineWithDetailsAsync(machineId);
        if (machine == null) throw new KeyNotFoundException("Machine not found.");

        var maintenance = machine.Maintenances.FirstOrDefault(m => m.Id == maintenanceId);
        if (maintenance == null) throw new KeyNotFoundException("Maintenance not found on this machine.");

        maintenance.CompleteMaintenance();
        await _unitOfWork.CommitAsync();
    }
}