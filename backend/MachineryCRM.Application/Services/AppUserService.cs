using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Interfaces;
using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Interfaces;

namespace MachineryCRM.Application.Services;

public class AppUserService : IAppUserService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AppUserService(IAppUserRepository appUserRepository, IUnitOfWork unitOfWork)
    {
        _appUserRepository = appUserRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppUserDto> CreateAsync(CreateAppUserDto dto)
    {
        var existingUser = await _appUserRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email is already in use.");

        var appUser = new AppUser(dto.FullName, dto.Email, dto.Password, dto.Role, true);

        await _appUserRepository.AddAsync(appUser);
        await _unitOfWork.CommitAsync();

        return new AppUserDto
        {
            Id = appUser.Id,
            FullName = appUser.FullName,
            Email = appUser.Email,
            Role = appUser.Role,
            IsActive = appUser.IsActive
        };
    }

    public async Task<AppUserDto?> GetByEmailAsync(string email)
    {
        var user = await _appUserRepository.GetByEmailAsync(email);
        if (user == null) return null;

        return new AppUserDto { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = user.Role, IsActive = user.IsActive };
    }

    public async Task<AppUserDto?> GetByIdAsync(Guid id)
    {
        var user = await _appUserRepository.GetByIdAsync(id);
        if (user == null) return null;

        return new AppUserDto { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = user.Role, IsActive = user.IsActive };
    }
}