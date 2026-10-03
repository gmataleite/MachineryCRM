using MachineryCRM.Domain.ValueObjects;

namespace MachineryCRM.Application.DTOs;

public class SiteDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public List<GeoPointDto> GeoPoints { get; set; } = new();
    public List<ContactDto> Contacts { get; set; } = new();
    public List<MachineDto> Machines { get; set; } = new();
}

public class CreateSiteDto
{
    public string Name { get; set; } = string.Empty;
    public Address? Address { get; set; }
}

public class UpdateSiteDto : CreateSiteDto { } 