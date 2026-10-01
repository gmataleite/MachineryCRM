using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Enums;
using Xunit;

namespace MachineryCRM.UnitTests.Domain;

public class SiteTests
{
    [Fact]
    public void CreateSite_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var site = new Site(Guid.NewGuid(), "Farm", "Belo Horizonte", "MG", "BR");
        
        // Assert
        Assert.Equal("Farm", site.Name);
        Assert.Equal("Belo Horizonte", site.City);
        Assert.Equal("MG", site.State);
        Assert.Equal("BR", site.Country);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var site = new Site(Guid.NewGuid(), "Farm", "Belo Horizonte", "MG", "BR");

        // Act
        site.UpdateDetails("New Farm", "São Paulo", "SP", "BR");

        // Assert
        Assert.Equal("New Farm", site.Name);
        Assert.Equal("São Paulo", site.City);
        Assert.Equal("SP", site.State);
        Assert.Equal("BR", site.Country);
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
