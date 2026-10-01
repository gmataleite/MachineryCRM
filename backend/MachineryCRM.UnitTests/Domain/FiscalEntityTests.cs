using MachineryCRM.Domain.Entities;

using Xunit;

namespace MachineryCRM.UnitTests.Domain;

public class FiscalEntityTests
{
    [Theory]
    [InlineData("Farm", "49938200036", "Rua Dois, 1122", "Bairro Dois", "Belo Horizonte", "MG", "30170-190", "BR",
                "Rua Três, 3344", "Bairro Três", "São Paulo", "SP", "01000-000", "BR")]
    [InlineData("Farm", "85150611000179", "Rua Dois, 1122", "Bairro Dois", "Belo Horizonte", "MG", "30170-190", "BR",
                "Rua Três, 3344", "Bairro Três", "São Paulo", "SP", "01000-000", "BR")]
    [InlineData("Farm", null, null, null, "Belo Horizonte", "MG", null, "BR",
                null, null, "São Paulo", "SP", null, "BR")]

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
        // Arrange & Act
        var fiscalEntity = new FiscalEntity(Guid.NewGuid(), name, taxId, billingAddressLine, billingNeighborhood, billingCity, billingState, billingPostalCode, billingCountryCode, shippingAddressLine, shippingNeighborhood, shippingCity, shippingState, shippingPostalCode, shippingCountryCode);
        
        // Assert
        Assert.Equal(name, fiscalEntity.Name);
        Assert.Equal(taxId, fiscalEntity.TaxId);
        Assert.Equal(billingAddressLine, fiscalEntity.BillingAddressLine);
        Assert.Equal(billingNeighborhood, fiscalEntity.BillingNeighborhood);
        Assert.Equal(billingCity, fiscalEntity.BillingCity);
        Assert.Equal(billingState, fiscalEntity.BillingState);
        Assert.Equal(billingPostalCode, fiscalEntity.BillingPostalCode);
        Assert.Equal(billingCountryCode, fiscalEntity.BillingCountryCode);
        Assert.Equal(shippingAddressLine, fiscalEntity.ShippingAddressLine);
        Assert.Equal(shippingNeighborhood, fiscalEntity.ShippingNeighborhood);
        Assert.Equal(shippingCity, fiscalEntity.ShippingCity);
        Assert.Equal(shippingState, fiscalEntity.ShippingState);
        Assert.Equal(shippingPostalCode, fiscalEntity.ShippingPostalCode);
        Assert.Equal(shippingCountryCode, fiscalEntity.ShippingCountryCode);
    }

    [Theory]
    [InlineData("Farm", "49938200036", "Rua Dois, 1122", "Bairro Dois", "Belo Horizonte", "MG", "30170-190", "BR",
                "Rua Três, 3344", "Bairro Três", "São Paulo", "SP", "01000-000", "BR")]
    [InlineData("Farm", "85150611000179", "Rua Dois, 1122", "Bairro Dois", "Belo Horizonte", "MG", "30170-190", "BR",
                "Rua Três, 3344", "Bairro Três", "São Paulo", "SP", "01000-000", "BR")]

    public void UpdateDetails_ShouldUpdatePropertiesCorrectly(string name, string? taxId, string? billingAddressLine, string? billingNeighborhood, string billingCity, string billingState, string? billingPostalCode, string billingCountryCode, string? shippingAddressLine, string? shippingNeighborhood, string shippingCity, string shippingState, string? shippingPostalCode, string shippingCountryCode)
    {
        // Arrange
        var fiscalEntity = new FiscalEntity(Guid.NewGuid(), "Farm", null, null, null, "Belo Horizonte", "MG", null, "BR",
                null, null, "São Paulo", "SP", null, "BR");

        // Act
        fiscalEntity.UpdateDetails(name, taxId, billingAddressLine, billingNeighborhood, billingCity, billingState, billingPostalCode, billingCountryCode, shippingAddressLine, shippingNeighborhood, shippingCity, shippingState, shippingPostalCode, shippingCountryCode);

        // Assert
        Assert.Equal(name, fiscalEntity.Name);
        Assert.Equal(taxId, fiscalEntity.TaxId);
        Assert.Equal(billingAddressLine, fiscalEntity.BillingAddressLine);
        Assert.Equal(billingNeighborhood, fiscalEntity.BillingNeighborhood);
        Assert.Equal(billingCity, fiscalEntity.BillingCity);
        Assert.Equal(billingState, fiscalEntity.BillingState);
        Assert.Equal(billingPostalCode, fiscalEntity.BillingPostalCode);
        Assert.Equal(billingCountryCode, fiscalEntity.BillingCountryCode);
        Assert.Equal(shippingAddressLine, fiscalEntity.ShippingAddressLine);
        Assert.Equal(shippingNeighborhood, fiscalEntity.ShippingNeighborhood);
        Assert.Equal(shippingCity, fiscalEntity.ShippingCity);
        Assert.Equal(shippingState, fiscalEntity.ShippingState);
        Assert.Equal(shippingPostalCode, fiscalEntity.ShippingPostalCode);
        Assert.Equal(shippingCountryCode, fiscalEntity.ShippingCountryCode);
    }

    [Fact]
    public void CreateFiscalEntity_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new FiscalEntity(Guid.NewGuid(), "", null, null, null, "Belo Horizonte", "MG", null, "BR",
                null, null, "São Paulo", "SP", null, "BR"));
        Assert.Equal("Fiscal entity name cannot be empty.", exception.Message);
    }

    [Fact]
    public void UpdateDetails_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var fiscalEntity = new FiscalEntity(Guid.NewGuid(), "Farm", null, null, null, "Belo Horizonte", "MG", null, "BR",
                null, null, "São Paulo", "SP", null, "BR");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => fiscalEntity.UpdateDetails("", null, null, null, "Belo Horizonte", "MG", null, "BR", null, null, "São Paulo", "SP", null, "BR"));
        Assert.Equal("Fiscal entity name cannot be empty.", exception.Message);
    }

    [Fact]
    public void CreateFiscalEntity_ShouldThrowArgumentException_WhenCnpjIsInvalid()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new FiscalEntity(Guid.NewGuid(), "Farm", "12345678901234", null, null, "Belo Horizonte", "MG", null, "BR",
                null, null, "São Paulo", "SP", null, "BR"));
        Assert.Equal("Invalid CNPJ format.", exception.Message);
    }

    [Fact]
    public void CreateFiscalEntity_ShouldThrowArgumentException_WhenCpfIsInvalid()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new FiscalEntity(Guid.NewGuid(), "Farm", "12345678901", null, null, "Belo Horizonte", "MG", null, "BR",
                null, null, "São Paulo", "SP", null, "BR"));
        Assert.Equal("Invalid CPF format.", exception.Message);
    }

    [Fact]
    public void CreateFiscalEntity_ShouldThrowArgumentException_WhenTaxIdIsInvalid()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new FiscalEntity(Guid.NewGuid(), "Farm", "abc85150611000179", null, null, "Belo Horizonte", "MG", null, "BR",
                null, null, "São Paulo", "SP", null, "BR"));
        Assert.Equal("Invalid tax ID format.", exception.Message);
    }
}
