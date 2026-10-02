using MachineryCRM.Domain.ValueObjects;

namespace MachineryCRM.Domain.Entities;

public class FiscalEntity : Entity
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; }
    public TaxId? TaxId { get; private set; }
    public Address? BillingAddress { get; private set; }
    public Address? ShippingAddress { get; private set; }
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();

    public FiscalEntity(Guid customerId, 
        string name, 
        TaxId? taxId,
        Address? billingAddress,
        Address? shippingAddress)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        CustomerId = customerId;
        Name = name;
        TaxId = taxId;
        BillingAddress = billingAddress;
        ShippingAddress = shippingAddress;
    }

    public void UpdateDetails(
        string name, 
        TaxId? taxId, 
        Address? billingAddress,
        Address? shippingAddress)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;    
        TaxId = taxId;
        BillingAddress = billingAddress;
        ShippingAddress = shippingAddress;;
    }
}