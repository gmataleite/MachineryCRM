namespace MachineryCRM.Domain.Entities;

public class Address
{
    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? District { get; private set; }
    public string Locality { get; private set; }
    public string AdministrativeArea { get; private set;}
    public string? PostalCode { get; private set; }

    // ISO 3166-1 alpha-2 two-letter country code (e.g., "US", "BR", "JP")
    public string CountryCode { get; private set; }

    // Primary Constructor
    public Address(
        string? addressLine1, 
        string? addressLine2, 
        string? district, 
        string locality, 
        string administrativeArea, 
        string? postalCode, 
        string countryCode)
    {
        if (string.IsNullOrWhiteSpace(locality)) 
            throw new ArgumentException("Locality is required.", nameof(locality));
        if (string.IsNullOrWhiteSpace(administrativeArea)) 
            throw new ArgumentException("Administrative Area is required.", nameof(administrativeArea));
        if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2) 
            throw new ArgumentException("A valid 2-letter ISO country code is required.", nameof(countryCode));

        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        District = district;
        Locality = locality;
        AdministrativeArea = administrativeArea;
        PostalCode = postalCode;
        CountryCode = countryCode.ToUpper();
    }

    // Computed Property to format the address nicely based on country formatting rules
    public string GetFormattedAddress()
    {
        var address = new[]
        {
            AddressLine1,
            AddressLine2,
            District,
            Locality,
            AdministrativeArea,
            PostalCode,
            CountryCode
        };

        return string.Join(Environment.NewLine, address.Where(p => !string.IsNullOrWhiteSpace(p)));
    }

        public string GetFormattedAddressInSingleLine()
    {
        var address = new[]
        {
            AddressLine1,
            AddressLine2,
            District,
            Locality,
            AdministrativeArea,
            PostalCode,
            CountryCode
        };

        // Removes empty or null lines dynamically
        return string.Join(", ", address.Where(p => !string.IsNullOrWhiteSpace(p)));
    }
    
}