namespace MachineryCRM.Application.DTOs;

public class SiteDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? State { get; set; } = string.Empty;
    public string? CountryCode { get; set; } = string.Empty;
    public string? Observations { get; set; }
    public List<GeoPointDto> GeoPoints { get; set; } = new();
    public List<ContactDto> Contacts { get; set; } = new();
    public List<MachineDto> Machines { get; set; } = new();
}

public class CreateSiteDto
{
    public string Name { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? State { get; set; } = string.Empty;
    public string? CountryCode { get; set; } = string.Empty;
    public string? Observations { get; set; }
}

public class UpdateSiteDto : CreateSiteDto { } 