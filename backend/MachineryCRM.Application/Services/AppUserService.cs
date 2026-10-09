using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Interfaces;
using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Interfaces;

namespace MachineryCRM.Application.Services;

public class AppUserService : IAppUserService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public AppUserService(IAppUserRepository appUserRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _appUserRepository = appUserRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<CreateAppUserResponse> CreateAsync(CreateAppUserDto dto)
    {
        var existingUser = await _appUserRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email is already in use.");

        string temporaryPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
        string hashedPassword = _passwordHasher.Hash(temporaryPassword);

        var appUser = new AppUser(dto.FullName, dto.Email, hashedPassword, dto.Role, true);

        await _appUserRepository.AddAsync(appUser);
        await _unitOfWork.CommitAsync();

        return new CreateAppUserResponse
        {
            Id = appUser.Id,
            FullName = appUser.FullName,
            Email = appUser.Email,
            Role = appUser.Role,
            IsActive = appUser.IsActive,
            MustChangePassword = appUser.MustChangePassword,
            TemporaryPassword = temporaryPassword
        };
    }

    public async Task<AppUserDto?> AuthenticateAsync(string email, string password)
    {
        var user = await _appUserRepository.GetByEmailAsync(email);
        if (user == null || !user.IsActive || !_passwordHasher.Verify(password, user.PasswordHash))
            return null;

        return Map(user);
    }

    public async Task ChangePasswordAsync(string email, string currentPassword, string newPassword)
    {
        var user = await _appUserRepository.GetByEmailAsync(email);
        if (user == null || !user.IsActive || !_passwordHasher.Verify(currentPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        user.UpdatePassword(_passwordHasher.Hash(newPassword));
        _appUserRepository.Update(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task<(AppUserDto User, string TemporaryPassword)> ResetPasswordAsync(string email)
    {
        var user = await _appUserRepository.GetByEmailAsync(email);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        string temporaryPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
        user.ForcePasswordChange(_passwordHasher.Hash(temporaryPassword));
        _appUserRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return (Map(user), temporaryPassword);
    }

    public async Task<AppUserDto?> GetByEmailAsync(string email)
    {
        var user = await _appUserRepository.GetByEmailAsync(email);
        if (user == null) return null;

        return Map(user);
    }

    public async Task<AppUserDto?> GetByIdAsync(Guid id)
    {
        var user = await _appUserRepository.GetByIdAsync(id);
        if (user == null) return null;

        return Map(user);
    }

    private static AppUserDto Map(AppUser user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role,
        IsActive = user.IsActive,
        MustChangePassword = user.MustChangePassword
    };
}