namespace MachineryCRM.Domain.Entities;

public class Technician : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string RegistrationNumber { get; private set; }

    public Technician(string name, string email, string registrationNumber)
    {
        Name = name;
        Email = email;
        RegistrationNumber = registrationNumber;
    }
}