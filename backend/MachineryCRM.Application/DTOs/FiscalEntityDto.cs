using MachineryCRM.Domain.ValueObjects;

namespace MachineryCRM.Application.DTOs;

public class FiscalEntityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TaxId? TaxId { get; set; }
    public Address? BillingAddress { get; set; } 
    public Address? ShippingAddress { get; set; } 
    public List<ContactDto> Contacts { get; set; } = new();
}

public class CreateFiscalEntityDto
{
    public string Name { get; set; } = string.Empty;
    public TaxId? TaxId { get; set; }
    public Address? BillingAddress { get; set; } 
    public Address? ShippingAddress { get; set; } 
}

public class UpdateFiscalEntityDto : CreateFiscalEntityDto { }