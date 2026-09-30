namespace MachineryCRM.Domain.Entities;

public class FiscalEntity : Entity
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public string? Cnpj { get; private set; }
    public string? Cpf { get; private set; }
    public string Locality { get; private set; }
    public string AdministrativeArea { get; private set; }
    public string CountryCode { get; private set; }
    public string? BillingAddress { get; private set; }
    public string? ShippingAddress { get; private set; }
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();

    public FiscalEntity(Guid customerId, string name, string? cnpj, string? cpf,
        string? billingAddressLine1, 
        string? billingAddressLine2, 
        string? billingDistrict, 
        string billingLocality, 
        string billingAdministrativeArea, 
        string? billingPostalCode, 
        string billingCountryCode,
        string? shippingAddressLine1, 
        string? shippingAddressLine2, 
        string? shippingDistrict, 
        string shippingLocality, 
        string shippingAdministrativeArea, 
        string? shippingPostalCode, 
        string shippingCountryCode)
    {
        CustomerId = customerId;
        Name = name;
        Cnpj = cnpj;
        Cpf = cpf;
        Locality = shippingLocality;
        AdministrativeArea = shippingAdministrativeArea;
        CountryCode = shippingCountryCode;
        
        BillingAddress = new Address(billingAddressLine1, billingAddressLine2, billingDistrict, billingLocality, billingAdministrativeArea, billingPostalCode, billingCountryCode).GetFormattedAddress();
        ShippingAddress = new Address(shippingAddressLine1, shippingAddressLine2, shippingDistrict, shippingLocality, shippingAdministrativeArea, shippingPostalCode, shippingCountryCode).GetFormattedAddress();
    }

    public void UpdateDetails(string name, string? cnpj, string? cpf,
        string? billingAddressLine1, 
        string? billingAddressLine2, 
        string? billingDistrict, 
        string billingLocality, 
        string billingAdministrativeArea, 
        string? billingPostalCode, 
        string billingCountryCode,
        string? shippingAddressLine1, 
        string? shippingAddressLine2, 
        string? shippingDistrict, 
        string shippingLocality, 
        string shippingAdministrativeArea, 
        string? shippingPostalCode, 
        string shippingCountryCode)
    {
        Name = name;
        Cnpj = cnpj;
        Cpf = cpf;
        Locality = shippingLocality;
        AdministrativeArea = shippingAdministrativeArea;
        CountryCode = shippingCountryCode;
        
        BillingAddress = new Address(billingAddressLine1, billingAddressLine2, billingDistrict, billingLocality, billingAdministrativeArea, billingPostalCode, billingCountryCode).GetFormattedAddress();
        ShippingAddress = new Address(shippingAddressLine1, shippingAddressLine2, shippingDistrict, shippingLocality, shippingAdministrativeArea, shippingPostalCode, shippingCountryCode).GetFormattedAddress();
    }
}