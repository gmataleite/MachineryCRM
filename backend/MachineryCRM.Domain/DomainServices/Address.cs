namespace MachineryCRM.Domain.DomainServices;

public class Address
{
    public string? AddressLine { get; private set; }
    public string? Neighborhood { get; private set; }
    public string City { get; private set; }
    public string State { get; private set;}
    public string? PostalCode { get; private set; }
    public string Country { get; private set; }

    public Address(
        string? addressLine, 
        string? neighborhood, 
        string city, 
        string state, 
        string? postalCode,
        string country)
    {
        if (string.IsNullOrWhiteSpace(city)) 
            throw new ArgumentException("City is required.", nameof(city));
        if (string.IsNullOrWhiteSpace(state)) 
            throw new ArgumentException("State is required.", nameof(state));
        if (string.IsNullOrWhiteSpace(country)) 
            throw new ArgumentException("Country is required.", nameof(country));

        AddressLine = addressLine;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
    }

    public string GetFormattedAddress()
    {
        var address = new[]
        {
            AddressLine,
            Neighborhood,
            City,
            State,
            PostalCode,
            Country
        };

        return string.Join(Environment.NewLine, address.Where(p => !string.IsNullOrWhiteSpace(p)));
    }

        public string GetFormattedAddressInSingleLine()
    {
        var address = new[]
        {
            AddressLine,
            Neighborhood,
            City,
            State,
            PostalCode,
            Country
        };

        return string.Join(", ", address.Where(p => !string.IsNullOrWhiteSpace(p)));
    }
    
}