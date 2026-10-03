namespace MachineryCRM.Domain.Entities;

public class TransferHistory : Entity
{
    public Guid MachineId { get; private set; }
    public Guid OriginSiteId { get; private set; }
    public Guid DestinationSiteId { get; private set; }
    public DateTime TransferDate { get; private set; }
    public string Reason { get; private set; } = null!;
    public string LoggedBy { get; private set; } = null!;

    public TransferHistory(Guid machineId, Guid originSiteId, Guid destinationSiteId, DateTime transferDate, string reason, string loggedBy)
    {
        MachineId = machineId;
        OriginSiteId = originSiteId;
        DestinationSiteId = destinationSiteId;
        TransferDate = transferDate;
        Reason = reason;
        LoggedBy = loggedBy;
    }
}