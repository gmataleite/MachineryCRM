namespace MachineryCRM.Application.DTOs;

public class CreateContactDto
{
    public Guid? SiteId { get; set; }
    public Guid? FiscalEntityId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
}