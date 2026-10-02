using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.ValueObjects;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.Entities;

public class FiscalEntityTests
{
    [Theory]
    [InlineData("Farm", "49938200036", "Rua Dois, 1122", "Bairro Dois", "Belo Horizonte", "MG", "30170-190", "BR",
                "Rua Três, 3344", "Bairro Três", "São Paulo", "SP", "01000-000", "BR")]
    [InlineData("Farm", "85150611000179", "Rua Dois, 1122", "Bairro Dois", "Belo Horizonte", "MG", "30170-190", "BR",
                "Rua Três, 3344", "Bairro Três", "São Paulo", "SP", "01000-000", "BR")]
    [InlineData("Farm", null, null, null, "Belo Horizonte", "MG", null, "BR",
                null, null, "São Paulo", "SP", null, "BR")]
    [InlineData("Farm", "12ABC345000188", "Rua Dois, 1122", "Bairro Dois", "Belo Horizonte", "MG", "30170-190", "BR",
                "Rua Três, 3344", "Bairro Três", "São Paulo", "SP", "01000-000", "BR")]

    public void CreateFiscalEntity_ShouldInitializePropertiesCorrectly(
        string name, 
        string? taxId, 
        string? billingAddressLine, 
        string? billingNeighborhood, 
        string billingCity, 
        string billingState, 
        string? billingPostalCode,
        string billingCountryCode, 
        string? shippingAddressLine, 
        string? shippingNeighborhood, 
        string shippingCity, 
        string shippingState, 
        string? shippingPostalCode,
        string shippingCountryCode)
    {
        // Arrange 
        var taxIdObj = string.IsNullOrWhiteSpace(taxId) ? null : new TaxId(taxId, billingCountryCode);
        var billingAddress = new Address(billingAddressLine, billingNeighborhood, billingCity, billingState, billingPostalCode, billingCountryCode);
        var shippingAddress = new Address(shippingAddressLine, shippingNeighborhood, shippingCity, shippingState, shippingPostalCode, shippingCountryCode);
        
        // Act
        var fiscalEntity = new FiscalEntity(Guid.NewGuid(), name, taxIdObj, billingAddress, shippingAddress);
        
        // Assert
        Assert.Equal(name, fiscalEntity.Name);
        Assert.Equal(taxId, fiscalEntity.TaxId?.Value);
        Assert.Equal(billingAddress, fiscalEntity.BillingAddress);
        Assert.Equal(shippingAddress, fiscalEntity.ShippingAddress);
    }

    [Theory]
    [InlineData("Farm", "49938200036", "Rua Dois, 1122", "Bairro Dois", "Belo Horizonte", "MG", "30170-190", "BR",
                "Rua Três, 3344", "Bairro Três", "São Paulo", "SP", "01000-000", "BR")]
    [InlineData("Farm", "85150611000179", "Rua Dois, 1122", "Bairro Dois", "Belo Horizonte", "MG", "30170-190", "BR",
                "Rua Três, 3344", "Bairro Três", "São Paulo", "SP", "01000-000", "BR")]
    [InlineData("Farm", "12ABC345000188", "Rua Dois, 1122", "Bairro Dois", "Belo Horizonte", "MG", "30170-190", "BR",
                "Rua Três, 3344", "Bairro Três", "São Paulo", "SP", "01000-000", "BR")]

    public void UpdateDetails_ShouldUpdatePropertiesCorrectly(string name, string? taxId, string? billingAddressLine, string? billingNeighborhood, string billingCity, string billingState, string? billingPostalCode, string billingCountryCode, string? shippingAddressLine, string? shippingNeighborhood, string shippingCity, string shippingState, string? shippingPostalCode, string shippingCountryCode)
    {
        // Arrange

        var taxIdObj = string.IsNullOrWhiteSpace(taxId) ? null : new TaxId(taxId, billingCountryCode);
        var billingAddress = new Address(billingAddressLine, billingNeighborhood, billingCity, billingState, billingPostalCode, billingCountryCode);
        var shippingAddress = new Address(shippingAddressLine, shippingNeighborhood, shippingCity, shippingState, shippingPostalCode, shippingCountryCode);
        var fiscalEntity = new FiscalEntity(Guid.NewGuid(), "Farm", null, null, null);

        // Act
        fiscalEntity.UpdateDetails(name, taxIdObj, billingAddress, shippingAddress);

        // Assert
        Assert.Equal(name, fiscalEntity.Name);
        Assert.Equal(taxId, fiscalEntity.TaxId?.Value);
        Assert.Equal(billingAddress, fiscalEntity.BillingAddress);
        Assert.Equal(shippingAddress, fiscalEntity.ShippingAddress);
    }

    [Fact]
    public void CreateFiscalEntity_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new FiscalEntity(Guid.NewGuid(), "", null, null, null));
        Assert.Equal("Name cannot be null or whitespace.", exception.Message);
    }

    [Fact]
    public void UpdateDetails_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var fiscalEntity = new FiscalEntity(Guid.NewGuid(), "Farm", null, null, null);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => fiscalEntity.UpdateDetails("", null, null, null));
        Assert.Equal("Name cannot be null or whitespace.", exception.Message);
    }
}
