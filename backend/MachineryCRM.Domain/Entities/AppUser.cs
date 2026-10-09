namespace MachineryCRM.Domain.Entities;

public class AppUser : Entity
{
    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public bool MustChangePassword { get; private set; }

    public AppUser(string fullName, string email, string passwordHash, UserRole role, bool isActive = true)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = isActive;
        MustChangePassword = true;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        MustChangePassword = false;
    }

    public void ForcePasswordChange(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        MustChangePassword = true;
    }

    public void UpdateRole(UserRole newRole)
    {
        Role = newRole;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

}