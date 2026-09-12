using MachineryCRM.Application.DTOs;

namespace MachineryCRM.Application.Interfaces;

public interface IMachineService
{
    Task<MachineDto> CreateAsync(CreateMachineDto dto);
    Task<MachineDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<MachineDto>> GetAllAsync();
}