namespace MachineryCRM.Application.DTOs;

public class CreateCustomerDto
{
    public string Name { get; set; } = string.Empty;
}

public class CustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Listas simplificadas para atender ao protótipo de UI
    public List<SiteDto> Sites { get; set; } = new();
    public List<FiscalEntityDto> FiscalEntities { get; set; } = new();
    public List<ContactDto> Contacts { get; set; } = new();
}

public class SiteDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Observations { get; set; }
}

public class FiscalEntityDto
{
    public Guid Id { get; set; }
    public string? SapPn { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}

public class ContactDto
{
    public Guid Id { get; set; }
    public Guid? SiteId { get; set; }
    public Guid? FiscalEntityId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
}