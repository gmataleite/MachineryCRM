namespace MachineryCRM.Domain.Entities;

public class GeoPoint : Entity
{
    public Guid SiteId { get; private set; }
    public string Description { get; private set; }
    public string Coordinates { get; private set; }
    public bool IsMachineLocation { get; private set; }
    public bool IsWaypoint { get; private set; }
    public bool IsOffice { get; private set; }

    public Site? Site { get; private set; }

    public GeoPoint(Guid siteId, string description, string coordinates)
    {
        SiteId = siteId;
        Description = description;
        Coordinates = coordinates;
    }
}