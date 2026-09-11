namespace MachineryCRM.Domain.Entities;

public class Machine : Entity
{
    public string SerialNumber { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }

    public Machine(string serialNumber, string model, int year)
    {
        SerialNumber = serialNumber;
        Model = model;
        Year = year;
    }
}