namespace MachineryCRM.Domain.Entities;

public class AppUser : Entity
{
    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string Role { get; private set; } = null!;
    public bool IsActive { get; private set; }

    public AppUser(string fullName, string email, string passwordHash, string role, bool isActive = true)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = isActive;
    }
}