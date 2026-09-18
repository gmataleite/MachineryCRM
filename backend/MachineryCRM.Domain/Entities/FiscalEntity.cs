namespace MachineryCRM.Domain.Entities;

public class FiscalEntity : Entity
{
    public Guid CustomerId { get; private set; }
    public string? SapPn { get; private set; }
    public string Name { get; private set; }
    public string? Cpf { get; private set; }
    public string? Cnpj { get; private set; }
    public string? Ie { get; private set; }
    public string Country { get; private set; }
    public string State { get; private set; }
    public string City { get; private set; }
    public string? FiscalAddress { get; private set; }
    public string? PostalAddress { get; private set; }
    public string? Observations { get; private set; }

    public Customer? Customer { get; private set; }
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();

    public FiscalEntity(Guid customerId, string name, string country, string state, string city)
    {
        CustomerId = customerId;
        Name = name;
        Country = country;
        State = state;
        City = city;
    }
}