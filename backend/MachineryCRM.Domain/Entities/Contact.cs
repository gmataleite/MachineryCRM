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

    public Contact(Guid customerId, string description, string? phone = null, string? email = null)
    {
        CustomerId = customerId;
        Description = description;
        Phone = phone;
        Email = email;
    }

    public void UpdateDetails(string description, string? phone, string? email, string? observations)
    {
        Description = description;
        Phone = phone;
        Email = email;
        Observations = observations;
    }

    public void ChangeSite(Guid? siteId)
    {
        SiteId = siteId;
        if (siteId.HasValue) 
            FiscalEntityId = null; 
    }

    public void ChangeFiscalEntity(Guid? fiscalEntityId)
    {
        FiscalEntityId = fiscalEntityId;
        if (fiscalEntityId.HasValue) 
            SiteId = null;
    }
}