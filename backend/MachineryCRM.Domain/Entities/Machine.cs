namespace MachineryCRM.Domain.Entities;

public class Machine : Entity
{
    public string SerialNumber { get; private set; }
    public string Model { get; private set; }
    public string Manufacturer { get; private set; }
    public int Year { get; private set; }

    public Machine(string serialNumber, string model, string manufacturer, int year)
    {
        SerialNumber = serialNumber;
        Model = model;
        Manufacturer = manufacturer;
        Year = year;
    }
}