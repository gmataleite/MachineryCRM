namespace MachineryCRM.Domain.Entities;

public class Site : Entity
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public string Country { get; private set; }
    public string State { get; private set; }
    public string City { get; private set; }
    public string? Observations { get; private set; }

    public Customer? Customer { get; private set; }
    public ICollection<GeoPoint> GeoPoints { get; private set; } = new List<GeoPoint>();
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();
    public ICollection<Machine> Machines { get; private set; } = new List<Machine>();

    public Site(Guid customerId, string name, string country, string state, string city)
    {
        CustomerId = customerId;
        Name = name;
        Country = country;
        State = state;
        City = city;
    }
}