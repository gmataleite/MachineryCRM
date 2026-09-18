namespace MachineryCRM.Domain.Entities;

public class Contact : Entity
{
    public Guid CustomerId { get; private set; }
    public Guid? SiteId { get; private set; }
    public Guid? FiscalEntityId { get; private set; }
    public string Description { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string? Observations { get; private set; }

    public Customer? Customer { get; private set; }
    public Site? Site { get; private set; }
    public FiscalEntity? FiscalEntity { get; private set; }

    public Contact(Guid customerId, string description)
    {
        CustomerId = customerId;
        Description = description;
    }
}