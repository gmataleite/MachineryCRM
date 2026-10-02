using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.ValueObjects;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.Entities;

public class SiteTests
{
    [Theory]
    [InlineData("Farm", "Belo Horizonte", "MG", "BR")]
    [InlineData("Farm", null, "SP", "BR")]
    [InlineData("Farm", "São Paulo", null, "BR")]
    [InlineData("Farm", "Rio de Janeiro", "RJ", null)]
    [InlineData("Farm", "", "", "")]
    [InlineData("Farm", null, null, null)]

    public void CreateSite_ShouldInitializePropertiesCorrectly(string name, string? city, string? state, string? countryCode)
    {
        // Arrange 
        var address = new Address(null, null, city, state, null, countryCode);
        
        // Act
        var site = new Site(Guid.NewGuid(), name, address);
        
        // Assert
        Assert.Equal(name, site.Name);
        Assert.Equal(address, site.Address);
    }

    [Theory]
    [InlineData("Farm", null, "SP", "BR")]
    [InlineData("Farm", "São Paulo", null, "BR")]
    [InlineData("Farm", "Rio de Janeiro", "RJ", null)]
    [InlineData("Farm", "", "", "")]
    [InlineData("Farm", null, null, null)]
    public void UpdateDetails_ShouldUpdatePropertiesCorrectly(string name, string? city, string? state, string? countryCode)
    {
        // Arrange
        var address = new Address(null, null, "Salvador", "BH", null, "BR");
        var site = new Site(Guid.NewGuid(), "Plantation", address);

        var newAddress = new Address(null, null, city, state, null, countryCode);

        // Act
        site.UpdateDetails(name, newAddress);

        // Assert
        Assert.Equal(name, site.Name);
        Assert.Equal(newAddress, site.Address);
    }

    [Fact]
    public void CreateSite_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Site(Guid.NewGuid(), "", null));
        Assert.Equal("Site name cannot be empty.", exception.Message);
    }

    [Fact]
    public void UpdateDetails_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var site = new Site(Guid.NewGuid(), "Farm", null);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => site.UpdateDetails("", null));
        Assert.Equal("Site name cannot be empty.", exception.Message);
    }
}
