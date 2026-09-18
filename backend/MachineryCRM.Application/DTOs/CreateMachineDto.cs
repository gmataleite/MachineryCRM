namespace MachineryCRM.Application.DTOs;

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