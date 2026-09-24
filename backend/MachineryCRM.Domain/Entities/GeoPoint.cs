namespace MachineryCRM.Domain.Entities;

using MachineryCRM.Domain.Enums;

public class GeoPoint : Entity
{
    public Guid SiteId { get; private set; }
    public string Description { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public GeoLocationType LocationType { get; private set; }
    public int? Order { get; private set; }
    public Site? Site { get; private set; }

    public GeoPoint(Guid siteId, string description, double latitude, double longitude, GeoLocationType locationType, int? order = null)
    {
        SiteId = siteId;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        LocationType = locationType;
        
        SetOrder(order);
    }

    public void UpdateDetails(string description, double latitude, double longitude)
    {
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void SetOrder(int? order = null)
    {
        switch (LocationType)
        {
            case GeoLocationType.Office:
                Order = -1;
                break;
            case GeoLocationType.MachineLocation:
                Order = 0;
                break;
            case GeoLocationType.Waypoint:
                Order = order;
                break;
        }
    }
}