namespace MachineryCRM.Domain.Entities;

public class Communication : Entity
{
    public Guid MachineId { get; private set; }
    public Guid ContactId { get; private set; }
    public Guid AppUserId { get; private set; }
    public DateTime InteractionDate { get; private set; }
    public string Channel { get; private set; } = null!;
    public string Summary { get; private set; } = null!;

    public Communication(Guid machineId, Guid contactId, Guid appUserId, DateTime interactionDate, string channel, string summary)
    {
        MachineId = machineId;
        ContactId = contactId;
        AppUserId = appUserId;
        InteractionDate = interactionDate;
        Channel = channel;
        Summary = summary;
    }
}