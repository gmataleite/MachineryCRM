using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Enums;
using MachineryCRM.Domain.ValueObjects;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.Entities;

public class SiteTests
{
    [Fact]
    public void CreateSite_ShouldInitializePropertiesCorrectly()
    {
        var customerId = Guid.NewGuid();
        var address = new Address(null, null, "BH", "MG", null, "BR");
        
        var site = new Site(customerId, "Farm", address);
        
        Assert.Equal(customerId, site.CustomerId);
        Assert.Equal("Farm", site.Name);
        Assert.Equal(address, site.Address);
    }   

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CreateSite_ShouldThrowArgumentException_WhenNameIsInvalid(string? name)
    {
        var exception = Assert.ThrowsAny<ArgumentException>(() => new Site(Guid.NewGuid(), name!, null));
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateProperties()
    {
        var site = new Site(Guid.NewGuid(), "Old", null);
        var newAddress = new Address(null, null, "SP", "SP", null, "BR");

        site.UpdateDetails("New", newAddress);

        Assert.Equal("New", site.Name);
        Assert.Equal(newAddress, site.Address);
    }

    [Fact]
    public void ReorderGeoPoints_ShouldUpdateOrder_WhenValidAndMatchesRules()
    {
        var site = new Site(Guid.NewGuid(), "Farm", null);
        var p1 = new GeoPoint(site.Id, "G1", 0, 0, 0, (GeoLocationType)0);
        var p2 = new GeoPoint(site.Id, "G2", 0, 0, 0, (GeoLocationType)1);
        
        site.GeoPoints.Add(p1);
        site.GeoPoints.Add(p2);

        var orderedIds = new List<Guid> { p1.Id, p2.Id };
        
        site.ReorderGeoPoints(orderedIds);

        Assert.Equal(0, p1.Order);
        Assert.Equal(1, p2.Order);
    }

    [Fact]
    public void ReorderGeoPoints_ShouldThrowInvalidOperationException_WhenCountMismatches()
    {
        var site = new Site(Guid.NewGuid(), "Farm", null);
        site.GeoPoints.Add(new GeoPoint(site.Id, "G1", 0, 0, 0, GeoLocationType.Waypoint));

        var exception = Assert.Throws<InvalidOperationException>(() => site.ReorderGeoPoints(new List<Guid>()));
        Assert.Contains("A lista de ordenação deve conter a exata quantidade", exception.Message);
    }

    [Fact]
    public void ReorderGeoPoints_ShouldThrowInvalidOperationException_WhenHierarchyIsViolated()
    {
        var site = new Site(Guid.NewGuid(), "Farm", null);
        var p1 = new GeoPoint(site.Id, "Machine", 0, 0, 0, (GeoLocationType)2); // Nível superior
        var p2 = new GeoPoint(site.Id, "Office", 0, 0, 0, (GeoLocationType)0);  // Nível inferior
        
        site.GeoPoints.Add(p1);
        site.GeoPoints.Add(p2);

        var orderedIds = new List<Guid> { p1.Id, p2.Id };

        var exception = Assert.Throws<InvalidOperationException>(() => site.ReorderGeoPoints(orderedIds));
        Assert.Contains("Ordem inválida", exception.Message);
    }

    [Fact]
    public void ReorderGeoPoints_ShouldThrowInvalidOperationException_WhenPointDoesNotBelongToSite()
    {
        var site = new Site(Guid.NewGuid(), "Farm", null);
        var p1 = new GeoPoint(site.Id, "G1", 0, 0, 0, GeoLocationType.Waypoint);
        site.GeoPoints.Add(p1);

        var wrongId = Guid.NewGuid();
        
        var exception = Assert.Throws<InvalidOperationException>(() => site.ReorderGeoPoints(new List<Guid> { wrongId }));
        Assert.Contains("não pertence a este Site", exception.Message);
    }
}