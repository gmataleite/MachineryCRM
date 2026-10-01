namespace MachineryCRM.Application.DTOs;

public class FiscalEntityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }
    public string? BillingAddressLine { get; set; } 
    public string? BillingNeighborhood { get; set; } 
    public string BillingCity { get; set; } = string.Empty;
    public string BillingState { get; set; } = string.Empty;
    public string? BillingPostalCode { get; set; } 
    public string BillingCountry { get; set; } = string.Empty;
    public string? ShippingAddressLine { get; set; } 
    public string? ShippingNeighborhood { get; set; } 
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingState { get; set; } = string.Empty;
    public string? ShippingPostalCode { get; set; } 
    public string ShippingCountry { get; set; } = string.Empty;
    public List<ContactDto> Contacts { get; set; } = new();
}

public class CreateFiscalEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }
    public string? BillingAddressLine { get; set; } 
    public string? BillingNeighborhood { get; set; }
    public string BillingCity { get; set; } = string.Empty;
    public string BillingState { get; set; } = string.Empty;
    public string? BillingPostalCode { get; set; } 
    public string BillingCountry { get; set; } = string.Empty;
    public string? ShippingAddressLine { get; set; } 
    public string? ShippingNeighborhood { get; set; } 
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingState { get; set; } = string.Empty;
    public string? ShippingPostalCode { get; set; } 
    public string ShippingCountry { get; set; } = string.Empty;
}

public class UpdateFiscalEntityDto : CreateFiscalEntityDto { }