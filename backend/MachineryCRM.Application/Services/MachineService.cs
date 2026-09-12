using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Interfaces;
using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Interfaces;

namespace MachineryCRM.Application.Services;

public class MachineService : IMachineService
{
    private readonly IMachineRepository _machineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MachineService(IMachineRepository machineRepository, IUnitOfWork unitOfWork)
    {
        _machineRepository = machineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MachineDto> CreateAsync(CreateMachineDto dto)
    {
        var existingMachine = await _machineRepository.GetBySerialNumberAsync(dto.SerialNumber);
        if (existingMachine != null)
        {
            throw new InvalidOperationException("A machine with this serial number already exists.");
        }

        var machine = new Machine(dto.SerialNumber, dto.Model, dto.Year);
        
        await _machineRepository.AddAsync(machine);
        await _unitOfWork.CommitAsync();

        return new MachineDto(machine.Id, machine.SerialNumber, machine.Model, machine.Year);
    }

    public async Task<MachineDto?> GetByIdAsync(Guid id)
    {
        var machine = await _machineRepository.GetByIdAsync(id);
        if (machine == null) return null;
        
        return new MachineDto(machine.Id, machine.SerialNumber, machine.Model, machine.Year);
    }

    public async Task<IEnumerable<MachineDto>> GetAllAsync()
    {
        var machines = await _machineRepository.GetAllAsync();
        return machines.Select(m => new MachineDto(m.Id, m.SerialNumber, m.Model, m.Year));
    }
}