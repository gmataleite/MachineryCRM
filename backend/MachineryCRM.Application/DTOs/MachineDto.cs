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