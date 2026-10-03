using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Enums;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.Entities;

public class GeoPointTests
{
    [Theory]
    [InlineData("Gate", -19.9167, -43.9345, 1, GeoLocationType.Waypoint)]
    [InlineData("Office", -23.5505, -46.6333, 5, GeoLocationType.Office)]
    [InlineData("Machine", -15.7942, -47.8822, 10, GeoLocationType.MachineLocation)]

    public void CreateGeoPoint_ShouldInitializePropertiesCorrectly(string description, double latitude, double longitude, int order, GeoLocationType locationType)
    {
        // Arrange & Act
        var geoPoint = new GeoPoint(Guid.NewGuid(), description, latitude, longitude, order, locationType);
        
        // Assert
        Assert.Equal(description, geoPoint.Description);
        Assert.Equal(latitude, geoPoint.Latitude);
        Assert.Equal(longitude, geoPoint.Longitude);
        Assert.Equal(order, geoPoint.Order);
        Assert.Equal(locationType, geoPoint.LocationType);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CreateGeoPoint_ShouldThrowArgumentException_CreateGeoPoint_ShouldThrow_WhenDescriptionIsNullOrWhitespace(string? description)
    {
        // Arrange, Act & Assert
        var exception = Assert.ThrowsAny<ArgumentException>(() => new GeoPoint(Guid.NewGuid(), description!, 0, 0, 0, GeoLocationType.Waypoint));
        Assert.Equal(nameof(GeoPoint.Description).ToLower(), exception.ParamName);
    }

    [Theory]
    [InlineData("Gate", -99.9167, -43.9345, 1, GeoLocationType.Waypoint)]
    [InlineData("Gate", 99.9167, -43.9345, 1, GeoLocationType.Waypoint)]
    public void CreateGeoPoint_ShouldThrowArgumentOutOfRangeException_WhenLatitudeIsInvalid(string description, double latitude, double longitude, int order, GeoLocationType locationType)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new GeoPoint(Guid.NewGuid(), description, latitude, longitude, order, locationType));
        Assert.Equal("latitude", exception.ParamName);
    }
    
    [Theory]
    [InlineData("Office", -19.9167, -186.6333, 5, GeoLocationType.Office)]
    [InlineData("Office", -19.9167, 186.6333, 5, GeoLocationType.Office)]
    public void CreateGeoPoint_ShouldThrowArgumentOutOfRangeException_WhenLongitudeIsInvalid(string description, double latitude, double longitude, int order, GeoLocationType locationType)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new GeoPoint(Guid.NewGuid(), description, latitude, longitude, order, locationType));
        Assert.Equal("longitude", exception.ParamName);
    }

    [Theory]
    [InlineData("Office", -19.9167, 45.8933, -2, GeoLocationType.Office)]
    public void CreateGeoPoint_ShouldThrowArgumentOutOfRangeException_WhenOrderIsNegative(string description, double latitude, double longitude, int order, GeoLocationType locationType)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new GeoPoint(Guid.NewGuid(), description, latitude, longitude, order, locationType));
        Assert.Equal("order", exception.ParamName);
    }

    [Theory]
    [InlineData(-90, -180, 0)]
    [InlineData(90, 180, 0)]
    [InlineData(0, 0, 0)]
    public void CreateGeoPoint_ShouldAccept_BoundaryValues(double lat, double lon, int order)
    {
        var point = new GeoPoint(Guid.NewGuid(), "Edge", lat, lon, order, GeoLocationType.Waypoint);

        Assert.Equal(lat, point.Latitude);
        Assert.Equal(lon, point.Longitude);
        Assert.Equal(order, point.Order);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void CreateGeoPoint_ShouldThrow_WhenLatitudeIsNotFinite(double latitude)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GeoPoint(Guid.NewGuid(), "Gate", latitude, 0, 0, GeoLocationType.Waypoint));

        Assert.Equal("latitude", ex.ParamName);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void CreateGeoPoint_ShouldThrow_WhenLongitudeIsNotFinite(double longitude)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GeoPoint(Guid.NewGuid(), "Gate", 0, longitude, 0, GeoLocationType.Waypoint));

        Assert.Equal("longitude", ex.ParamName);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateProperties()
    {
        var point = new GeoPoint(Guid.NewGuid(), "Gate", 10, 10, 1, GeoLocationType.Waypoint);

        point.UpdateDetails("New", 20, 30, 7);

        Assert.Equal("New", point.Description);
        Assert.Equal(20, point.Latitude);
        Assert.Equal(30, point.Longitude);
        Assert.Equal(7, point.Order);
    }

    [Fact]
    public void UpdateDetails_ShouldNotChangeState_WhenInvalid()
    {
        var point = new GeoPoint(Guid.NewGuid(), "Gate", 10, 10, 1, GeoLocationType.Waypoint);

        Assert.Throws<ArgumentOutOfRangeException>(() => point.UpdateDetails("New", 999, 30, 7));

        Assert.Equal("Gate", point.Description);
        Assert.Equal(10, point.Latitude);
    }

    [Fact]
    public void ChangeOrder_ShouldUpdateOrder()
    {
        var point = new GeoPoint(Guid.NewGuid(), "Gate", 10, 10, 1, GeoLocationType.Waypoint);

        point.ChangeOrder(4);

        Assert.Equal(4, point.Order);
    }

    [Fact]
    public void ChangeOrder_ShouldThrow_WhenNegative()
    {
        var point = new GeoPoint(Guid.NewGuid(), "Gate", 10, 10, 1, GeoLocationType.Waypoint);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => point.ChangeOrder(-1));
        Assert.Equal("newOrder", ex.ParamName);
    }
}
