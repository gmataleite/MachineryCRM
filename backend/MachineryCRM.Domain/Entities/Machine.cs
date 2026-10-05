namespace MachineryCRM.Domain.Entities;

public class Machine : Entity
{
    public Guid SiteId { get; private set; } 
    public string SerialNumber { get; private set; } = null!;
    public string Model { get; private set; } = null!;
    public string? Brand { get; private set; }
    public string? Status { get; private set; }
    public DateTime AcquisitionDate { get; private set; }

    public ICollection<TransferHistory> TransferHistories { get; private set; } = new List<TransferHistory>();
    public ICollection<Maintenance> Maintenances { get; private set; } = new List<Maintenance>();
    public ICollection<Communication> Communications { get; private set; } = new List<Communication>();

    public Machine(Guid siteId, string serialNumber, string model, string? brand, string? status, DateTime acquisitionDate)
    {
        SiteId = siteId;
        
        ArgumentException.ThrowIfNullOrWhiteSpace(serialNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        SerialNumber = serialNumber;
        Model = model;
        Brand = brand;
        Status = status;
        AcquisitionDate = acquisitionDate;
    }
}