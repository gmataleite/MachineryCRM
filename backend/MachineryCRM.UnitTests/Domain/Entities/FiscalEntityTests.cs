using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.ValueObjects;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.Entities;

public class FiscalEntityTests
{
    [Fact]
    public void CreateFiscalEntity_ShouldInitializePropertiesCorrectly()
    {
        var customerId = Guid.NewGuid();
        var taxId = new TaxId("49938200036", "BR");
        var address = new Address("Rua 1", "Bairro", "BH", "MG", "30000-000", "BR");
        
        var fiscalEntity = new FiscalEntity(customerId, "Farm", taxId, address, address);
        
        Assert.Equal(customerId, fiscalEntity.CustomerId);
        Assert.Equal("Farm", fiscalEntity.Name);
        Assert.Equal(taxId, fiscalEntity.TaxId);
        Assert.Equal(address, fiscalEntity.BillingAddress);
        Assert.Equal(address, fiscalEntity.ShippingAddress);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CreateFiscalEntity_ShouldThrowArgumentException_WhenNameIsInvalid(string? name)
    {
        var exception = Assert.ThrowsAny<ArgumentException>(() => new FiscalEntity(Guid.NewGuid(), name!, null, null, null));
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateProperties()
    {
        var entity = new FiscalEntity(Guid.NewGuid(), "Old", null, null, null);
        var newAddress = new Address(null, null, "SP", "SP", null, "BR");
        
        entity.UpdateDetails("New", null, newAddress, null);

        Assert.Equal("New", entity.Name);
        Assert.Equal(newAddress, entity.BillingAddress);
        Assert.Null(entity.ShippingAddress);
    }
}