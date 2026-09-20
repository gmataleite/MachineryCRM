namespace MachineryCRM.Application.DTOs;

public class CreateSiteDto
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = "Brasil";
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Observations { get; set; }
}
