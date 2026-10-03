namespace MachineryCRM.Application.DTOs;

public class TransferHistoryDto
{
    public Guid Id { get; set; }
    public Guid MachineId { get; set; }
    public Guid OriginSiteId { get; set; }
    public Guid DestinationSiteId { get; set; }
    public DateTime TransferDate { get; set; }
    public string Reason { get; set; } = null!;
    public string LoggedBy { get; set; } = null!;
}

public class CreateTransferHistoryDto
{
    public Guid MachineId { get; set; }
    public Guid DestinationSiteId { get; set; }
    public string Reason { get; set; } = null!;
    public string LoggedBy { get; set; } = null!;
}