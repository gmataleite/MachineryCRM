namespace MachineryCRM.Domain.Entities;

public class Contact : Entity
{
    public Guid CustomerId { get; private set; }
    public Guid? SiteId { get; private set; }
    public Guid? FiscalEntityId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string? Observations { get; private set; }


    private Contact() { }
    public Contact(Guid customerId, string name, string? phone, string? email, string? observations)
    {
        CustomerId = customerId;

        UpdateDetails(name, phone, email, observations);
    }

    public void UpdateDetails(string name, string? phone, string? email, string? observations)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;

        SetPhone(phone);
        SetEmail(email);

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

    private void SetEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            Email = null;
            return;
        }

        if (!System.Net.Mail.MailAddress.TryCreate(email, out var addr) || addr.Address != email || !addr.Host.Contains("."))
        {
            throw new FormatException("Invalid email format.");
        }

        Email = email;
    }

    private void SetPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            Phone = null;
            return;
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+?[0-9]\d{1,14}$"))
        {
            throw new FormatException("Invalid phone number format.");
        }

        Phone = phone;
    }

}