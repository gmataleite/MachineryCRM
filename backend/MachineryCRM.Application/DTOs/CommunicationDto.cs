namespace MachineryCRM.Application.DTOs;

public class CommunicationDto
{
    public Guid Id { get; set; }
    public Guid MachineId { get; set; }
    public Guid ContactId { get; set; }
    public Guid AppUserId { get; set; }
    public DateTime InteractionDate { get; set; }
    public string Channel { get; set; } = null!;
    public string Summary { get; set; } = null!;
}

public class CreateCommunicationDto
{
    public Guid MachineId { get; set; }
    public Guid ContactId { get; set; }
    public Guid AppUserId { get; set; }
    public DateTime InteractionDate { get; set; }
    public string Channel { get; set; } = null!;
    public string Summary { get; set; } = null!;
}