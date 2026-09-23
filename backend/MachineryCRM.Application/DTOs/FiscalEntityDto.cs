namespace MachineryCRM.Application.DTOs;

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

public class CreateFiscalEntityDto
{
    public string? SapPn { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}

public class UpdateFiscalEntityDto : CreateFiscalEntityDto { }