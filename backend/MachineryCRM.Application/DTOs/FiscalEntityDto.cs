namespace MachineryCRM.Application.DTOs;

public class FiscalEntityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }
    public string Locality { get; set; } = string.Empty;
    public string AdministrativeArea { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string? BillingAddress { get; set; }
    public string? ShippingAddress { get; set; }
}

public class CreateFiscalEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }
    public string Locality { get; set; } = string.Empty;
    public string AdministrativeArea { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string? BillingAddress { get; set; }
    public string? ShippingAddress { get; set; }
}

public class UpdateFiscalEntityDto : CreateFiscalEntityDto { }