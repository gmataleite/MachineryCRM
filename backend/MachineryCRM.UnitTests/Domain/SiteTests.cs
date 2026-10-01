using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Enums;
using Xunit;

namespace MachineryCRM.UnitTests.Domain;

public class SiteTests
{
    [Theory]
    [InlineData("Farm", "Belo Horizonte", "MG", "Brasil")]
    [InlineData("Farm", null, "SP", "Brasil")]
    [InlineData("Farm", "São Paulo", null, "Brasil")]
    [InlineData("Farm", "Rio de Janeiro", "RJ", null)]
    [InlineData("Farm", "", "", "")]
    [InlineData("Farm", null, null, null)]

    public void CreateSite_ShouldInitializePropertiesCorrectly(string name, string? city, string? state, string? country)
    {
        // Arrange & Act
        var site = new Site(Guid.NewGuid(), name, city, state, country);
        
        // Assert
        Assert.Equal(name, site.Name);
        Assert.Equal(city, site.City);
        Assert.Equal(state, site.State);
        Assert.Equal(country, site.Country);
    }

    [Theory]
    [InlineData("Farm", null, "SP", "Brasil")]
    [InlineData("Farm", "São Paulo", null, "Brasil")]
    [InlineData("Farm", "Rio de Janeiro", "RJ", null)]
    [InlineData("Farm", "", "", "")]
    [InlineData("Farm", null, null, null)]
    public void UpdateDetails_ShouldUpdatePropertiesCorrectly(string name, string? city, string? state, string? country)
    {
        // Arrange
        var site = new Site(Guid.NewGuid(), "Plantation", "Salvador", "BH", "Brasil");

        // Act
        site.UpdateDetails(name, city, state, country);

        // Assert
        Assert.Equal(name, site.Name);
        Assert.Equal(city, site.City);
        Assert.Equal(state, site.State);
        Assert.Equal(country, site.Country);
    }

    [Fact]
    public void CreateSite_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Site(Guid.NewGuid(), "", "Belo Horizonte", "MG", "BR"));
        Assert.Equal("Site name cannot be empty.", exception.Message);
    }

    [Fact]
    public void UpdateDetails_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var site = new Site(Guid.NewGuid(), "Farm", "Belo Horizonte", "MG", "BR");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => site.UpdateDetails("", "São Paulo", "SP", "BR"));
        Assert.Equal("Site name cannot be empty.", exception.Message);
    }
}
