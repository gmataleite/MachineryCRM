namespace MachineryCRM.Domain.Entities;

public class Machine : Entity
{
    public Guid SiteId { get; private set; } 
    public string SerialNumber { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }

    public Site? Site { get; private set; } 

    public Machine(Guid siteId, string serialNumber, string model, int year)
    {
        SiteId = siteId;
        SerialNumber = serialNumber;
        Model = model;
        Year = year;
    }
}