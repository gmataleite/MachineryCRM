namespace MachineryCRM.Application.DTOs;

using MachineryCRM.Domain.Enums;

public class GeoPointDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public GeoLocationType LocationType { get; set; }
    public int? Order { get; set; }
}

public class CreateGeoPointDto
{
    public string Description { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public GeoLocationType LocationType { get; set; }
    public int? Order { get; set; }
}

public class UpdateGeoPointDto
{
    public string Description { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class ReorderGeoPointDto
{
    public Guid Id { get; set; }
    public int Order { get; set; }
}