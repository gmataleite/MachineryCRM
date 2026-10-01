namespace MachineryCRM.Domain.Entities;

public class Site : Entity
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? CountryCode { get; private set; }
    public string? Observations { get; private set; }
    public ICollection<GeoPoint> GeoPoints { get; private set; } = new List<GeoPoint>();
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();
    public ICollection<Machine> Machines { get; private set; } = new List<Machine>();

    public Site(Guid customerId, 
        string name,
        string? city, 
        string? state, 
        string? countryCode,
        string? observations = null)
    {
        CustomerId = customerId;
        ValidateName(name);
        Name = name;
        City = city;
        State = state;
        CountryCode = countryCode;
        Observations = observations;
    }

    public void UpdateDetails(
        string name,
        string? city, 
        string? state, 
        string? countryCode,
        string? observations = null)
    {
        ValidateName(name);
        Name = name;
        City = city;
        State = state;
        CountryCode = countryCode;
        Observations = observations;
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Site name cannot be empty.");
    }
}
