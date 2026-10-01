namespace MachineryCRM.Domain.Entities;

public class FiscalEntity : Entity
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public string? TaxId { get; private set; }
    public string? BillingAddressLine { get; private set; }
    public string? BillingNeighborhood { get; private set; }
    public string BillingCity { get; private set; }
    public string BillingState { get; private set; }
    public string? BillingPostalCode { get; private set; }
    public string BillingCountryCode { get; private set; }
    public string? ShippingAddressLine { get; private set; }
    public string? ShippingNeighborhood { get; private set; }
    public string ShippingCity { get; private set; }
    public string ShippingState { get; private set; }
    public string? ShippingPostalCode { get; private set; }
    public string ShippingCountryCode { get; private set; }
    public string? BillingAddress { get; private set; }
    public string? ShippingAddress { get; private set; }
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();

    public FiscalEntity(Guid customerId, 
        string name, 
        string? taxId, 
        string? billingAddressLine, 
        string? billingNeighborhood, 
        string billingCity, 
        string billingState, 
        string? billingPostalCode,
        string billingCountryCode, 
        string? shippingAddressLine, 
        string? shippingNeighborhood, 
        string shippingCity, 
        string shippingState, 
        string? shippingPostalCode,
        string shippingCountryCode)
    {
        CustomerId = customerId;
        Name = name;
        TaxId = taxId;
        BillingAddressLine = billingAddressLine;
        BillingNeighborhood = billingNeighborhood;
        BillingCity = billingCity;
        BillingState = billingState;
        BillingPostalCode = billingPostalCode;
        BillingCountryCode = billingCountryCode;
        ShippingAddressLine = shippingAddressLine;
        ShippingNeighborhood = shippingNeighborhood;
        ShippingCity = shippingCity;
        ShippingState = shippingState;
        ShippingPostalCode = shippingPostalCode;
        ShippingCountryCode = shippingCountryCode;
    }

    public void UpdateDetails(
        string name, 
        string? taxId, 
        string? billingAddressLine, 
        string? billingNeighborhood, 
        string billingCity, 
        string billingState, 
        string? billingPostalCode,
        string billingCountryCode, 
        string? shippingAddressLine, 
        string? shippingNeighborhood, 
        string shippingCity, 
        string shippingState, 
        string? shippingPostalCode,
        string shippingCountryCode)
    {
        Name = name;
        TaxId = taxId;
        BillingAddressLine = billingAddressLine;
        BillingNeighborhood = billingNeighborhood;
        BillingCity = billingCity;
        BillingState = billingState;
        BillingPostalCode = billingPostalCode;
        BillingCountryCode = billingCountryCode;
        ShippingAddressLine = shippingAddressLine;
        ShippingNeighborhood = shippingNeighborhood;
        ShippingCity = shippingCity;
        ShippingState = shippingState;
        ShippingPostalCode = shippingPostalCode;
        ShippingCountryCode = shippingCountryCode;
    }

}