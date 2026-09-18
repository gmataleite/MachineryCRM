namespace MachineryCRM.Application.DTOs;

public class CustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Listas simplificadas para atender ao protótipo de UI
    public List<SiteDto> Sites { get; set; } = new();
    public List<FiscalEntityDto> FiscalEntities { get; set; } = new();
    public List<ContactDto> Contacts { get; set; } = new();
}