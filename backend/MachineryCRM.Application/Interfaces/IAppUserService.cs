using MachineryCRM.Application.DTOs;

namespace MachineryCRM.Application.Interfaces;

public interface IAppUserService
{
    Task<AppUserDto> CreateAsync(CreateAppUserDto dto);
    Task<AppUserDto?> GetByEmailAsync(string email);
    Task<AppUserDto?> GetByIdAsync(Guid id);
}