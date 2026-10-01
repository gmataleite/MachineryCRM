namespace MachineryCRM.Domain.Entities;

public class FiscalEntity : Entity
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public string? Cnpj { get; private set; }
    public string? Cpf { get; private set; }
    public string? BillingAddressLine { get; private set; }
    public string? BillingNeighborhood { get; private set; }
    public string BillingCity { get; private set; }
    public string BillingState { get; private set; }
    public string? BillingPostalCode { get; private set; }
    public string BillingCountry { get; private set; }
    public string? ShippingAddressLine { get; private set; }
    public string? ShippingNeighborhood { get; private set; }
    public string ShippingCity { get; private set; }
    public string ShippingState { get; private set; }
    public string? ShippingPostalCode { get; private set; }
    public string ShippingCountry { get; private set; }
    public string? BillingAddress { get; private set; }
    public string? ShippingAddress { get; private set; }
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();

    public FiscalEntity(Guid customerId, string name, string? cnpj, string? cpf,
        string? billingAddressLine, 
        string? billingNeighborhood, 
        string billingCity, 
        string billingState, 
        string? billingPostalCode,
        string billingCountry, 
        string? shippingAddressLine, 
        string? shippingNeighborhood, 
        string shippingCity, 
        string shippingState, 
        string? shippingPostalCode,
        string shippingCountry)
    {
        CustomerId = customerId;
        Name = name;
        Cnpj = cnpj;
        Cpf = cpf;
        BillingAddressLine = billingAddressLine;
        BillingNeighborhood = billingNeighborhood;
        BillingCity = billingCity;
        BillingState = billingState;
        BillingPostalCode = billingPostalCode;
        BillingCountry = billingCountry;
        ShippingAddressLine = shippingAddressLine;
        ShippingNeighborhood = shippingNeighborhood;
        ShippingCity = shippingCity;
        ShippingState = shippingState;
        ShippingPostalCode = shippingPostalCode;
        ShippingCountry = shippingCountry;
    }

    public void UpdateDetails(string name, string? cnpj, string? cpf,
        string? billingAddressLine, 
        string? billingNeighborhood, 
        string billingCity, 
        string billingState, 
        string? billingPostalCode,
        string billingCountry, 
        string? shippingAddressLine, 
        string? shippingNeighborhood, 
        string shippingCity, 
        string shippingState, 
        string? shippingPostalCode,
        string shippingCountry)
    {
        Name = name;
        Cnpj = cnpj;
        Cpf = cpf;
        BillingAddressLine = billingAddressLine;
        BillingNeighborhood = billingNeighborhood;
        BillingCity = billingCity;
        BillingState = billingState;
        BillingPostalCode = billingPostalCode;
        BillingCountry = billingCountry;
        ShippingAddressLine = shippingAddressLine;
        ShippingNeighborhood = shippingNeighborhood;
        ShippingCity = shippingCity;
        ShippingState = shippingState;
        ShippingPostalCode = shippingPostalCode;
        ShippingCountry = shippingCountry;
    }

}