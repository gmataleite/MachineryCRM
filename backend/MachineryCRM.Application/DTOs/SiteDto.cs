namespace MachineryCRM.Application.DTOs;

public class SiteDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Locality { get; set; } = string.Empty;
    public string AdministrativeArea { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string? Observations { get; set; }
    public List<GeoPointDto> GeoPoints { get; set; } = new List<GeoPointDto>();
}

public class CreateSiteDto
{
    public string Name { get; set; } = string.Empty;
    public string Locality { get; set; } = string.Empty;
    public string AdministrativeArea { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string? Observations { get; set; }
    public List<GeoPointDto> GeoPoints { get; set; } = new List<GeoPointDto>();
}

public class UpdateSiteDto : CreateSiteDto { } 