using MachineryCRM.Application.DTOs;

namespace MachineryCRM.Application.Interfaces;

public interface IAppUserService
{
    // --- CRUD ---
    Task<IEnumerable<AppUserDto>> GetAllAsync();
    Task<AppUserDto?> GetByIdAsync(Guid id);
    Task<AppUserDto?> GetByEmailAsync(string email);
    Task<CreateAppUserResponse> CreateAsync(CreateAppUserDto dto);
    Task UpdateAsync(Guid id, UpdateAppUserDto dto);
    Task DeactivateAsync(Guid id);

    // --- Autenticação e Senhas ---
    Task<AppUserDto?> AuthenticateAsync(string email, string password);
    Task ChangePasswordAsync(string email, string currentPassword, string newPassword);
    Task<(AppUserDto User, string TemporaryPassword)> ResetPasswordByIdAsync(Guid id); 
    Task<(AppUserDto User, string TemporaryPassword)> ReactivateAndResetPasswordAsync(Guid id);
}