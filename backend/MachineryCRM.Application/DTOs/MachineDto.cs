namespace MachineryCRM.Application.DTOs;

public class MachineDto
{
    public Guid Id { get; set; }
    public Guid SiteId { get; set; }
    public string SerialNumber { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Brand { get; set; }
    public string? Status { get; set; }
    public DateTime AcquisitionDate { get; set; }
}

public class CreateMachineDto
{
    public Guid SiteId { get; set; } 
    public string SerialNumber { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Brand { get; set; }
    public string? Status { get; set; }
    public DateTime AcquisitionDate { get; set; }
}