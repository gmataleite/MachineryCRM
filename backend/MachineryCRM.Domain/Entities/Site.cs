namespace MachineryCRM.Domain.Entities;

public class Site : Entity
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public string Locality { get; private set; }
    public string AdministrativeArea { get; private set; }
    public string CountryCode { get; private set; }
    public string? Observations { get; private set; }
    public ICollection<GeoPoint> GeoPoints { get; private set; } = new List<GeoPoint>();
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();
    public ICollection<Machine> Machines { get; private set; } = new List<Machine>();

    public Site(Guid customerId, string name,
        string shippingLocality, 
        string shippingAdministrativeArea, 
        string shippingCountryCode,
        string? observations = null)
    {
        CustomerId = customerId;
        Name = name;
        Locality = shippingLocality;
        AdministrativeArea = shippingAdministrativeArea;
        CountryCode = shippingCountryCode;
        Observations = observations;
    }

    public void UpdateDetails(string name,
        string shippingLocality, 
        string shippingAdministrativeArea, 
        string shippingCountryCode,
        string? observations = null)
    {
        Name = name;
        Locality = shippingLocality;
        AdministrativeArea = shippingAdministrativeArea;
        CountryCode = shippingCountryCode;
        Observations = observations;
    }
}
