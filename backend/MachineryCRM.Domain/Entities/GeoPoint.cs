namespace MachineryCRM.Domain.Entities;

using MachineryCRM.Domain.Enums;

public class GeoPoint : Entity
{
    public Guid SiteId { get; private set; }
    public string Description { get; private set; } = null!;
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public int Order { get; private set; }
    public GeoLocationType LocationType { get; private set; }

    private GeoPoint() { } 
    public GeoPoint(Guid siteId, string description, double latitude, double longitude, int order, GeoLocationType locationType)
    {
        SiteId = siteId;
        LocationType = locationType;
        
        UpdateDetails(description, latitude, longitude, order);
    }

    public void UpdateDetails(string description, double latitude, double longitude, int order)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentOutOfRangeException.ThrowIfNegative(order);

        if (!double.IsFinite(latitude) || latitude is < -90 or > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude), $"{nameof(latitude)} must be between -90 and 90.");

        if (!double.IsFinite(longitude) || longitude is < -180 or > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude), $"{nameof(longitude)} must be between -180 and 180.");

        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        Order = order;
    }

    public void ChangeOrder(int newOrder)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(newOrder);
        Order = newOrder;
    }
}