namespace MachineryCRM.Domain.ValueObjects;

public class Address
{
    public string? AddressLine { get; private set; }
    public string? Neighborhood { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set;}
    public string? PostalCode { get; private set; }
    public string? CountryCode { get; private set; }

    public Address(
        string? addressLine, 
        string? neighborhood, 
        string? city, 
        string? state, 
        string? postalCode,
        string? countryCode)
    {
        AddressLine = addressLine;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        PostalCode = postalCode;
        CountryCode = countryCode;
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
            CountryCode
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
            CountryCode
        };

        return string.Join(", ", address.Where(p => !string.IsNullOrWhiteSpace(p)));
    }
    
    public override string ToString() => GetFormattedAddressInSingleLine();

    public static implicit operator string?(Address? address) => address?.GetFormattedAddressInSingleLine();
}