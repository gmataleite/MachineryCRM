namespace MachineryCRM.Application.DTOs;

public class MachineDto
{
    public Guid Id { get; set; }
    public string SerialNumber { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public Guid SiteId { get; set; }

    public MachineDto(Guid id, string serialNumber, string model, int year, Guid siteId)
    {
        Id = id;
        SerialNumber = serialNumber;
        Model = model;
        Year = year;
        SiteId = siteId;
    }
}

public class CreateMachineDto
{
    public string SerialNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public Guid SiteId { get; set; } 
    
    public CreateMachineDto(Guid siteId, string serialNumber, string model, int year)
    {
        SiteId = siteId;
        SerialNumber = serialNumber;
        Model = model;
        Year = year;
    }
}