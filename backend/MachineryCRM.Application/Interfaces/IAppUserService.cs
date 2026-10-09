using MachineryCRM.Application.DTOs;

namespace MachineryCRM.Application.Interfaces;

public interface IAppUserService
{
    Task<CreateAppUserResponse> CreateAsync(CreateAppUserDto dto);
    Task<AppUserDto?> AuthenticateAsync(string email, string password);
    Task ChangePasswordAsync(string email, string currentPassword, string newPassword);
    Task<(AppUserDto User, string TemporaryPassword)> ResetPasswordAsync(string email);
    Task<AppUserDto?> GetByEmailAsync(string email);
    Task<AppUserDto?> GetByIdAsync(Guid id);
}