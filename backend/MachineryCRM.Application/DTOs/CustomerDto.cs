namespace MachineryCRM.Application.DTOs;

public class CustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<SiteDto> Sites { get; set; } = new();
    public List<FiscalEntityDto> FiscalEntities { get; set; } = new();
    public List<ContactDto> Contacts { get; set; } = new();
}
public class CreateCustomerDto { public string Name { get; set; } = string.Empty; }
public class UpdateCustomerDto { public string Name { get; set; } = string.Empty; }