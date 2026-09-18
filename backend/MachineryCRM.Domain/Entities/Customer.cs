namespace MachineryCRM.Domain.Entities;

public class Customer : Entity
{
    public string Name { get; private set; }

    public ICollection<FiscalEntity> FiscalEntities { get; private set; } = new List<FiscalEntity>();
    public ICollection<Site> Sites { get; private set; } = new List<Site>();
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();

    public Customer(string name)
    {
        Name = name;
    }
}