using MachineryCRM.Domain.ValueObjects;

namespace MachineryCRM.Domain.Entities;

public class Site : Entity
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public Address? Address { get; private set; }
    public string? Observations { get; private set; }
    public ICollection<GeoPoint> GeoPoints { get; private set; } = new List<GeoPoint>();
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();
    public ICollection<Machine> Machines { get; private set; } = new List<Machine>();

    public Site(Guid customerId, 
        string name,
        Address? address,
        string? observations = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        CustomerId = customerId;
        Name = name;
        Address = address;
        Observations = observations;
    }

    public void UpdateDetails(
        string name,
        Address? address,
        string? observations = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Address = address;
        Observations = observations;
    }
}
