using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.ValueObjects;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.ValueObjects;

public class TaxIdTests
{
    [Theory]
    [InlineData("49938200036", "BR")]
    [InlineData("85150611000179", "BR")]
    [InlineData("12ABC345000188", "BR")]
    [InlineData("11111", "AA")]
    public void CreateTaxId_ShouldInitializePropertiesCorrectly(string value, string countryCode)
    {
        // Arrange & Act
        var taxId = new TaxId(value, countryCode);

        // Assert
        Assert.Equal(value, taxId.Value);
    }

    [Theory]
    [InlineData("12345678901", "BR")]
    [InlineData("abc85150611000179", "BR")]
    [InlineData("12ABC3450001XX", "BR")]
    [InlineData("11111", "BR")]

    public void CreateTaxId_ShouldThrowArgumentException_WhenIdIsInvalid(string value, string countryCode)
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new TaxId(value, countryCode));
        Assert.Equal("Invalid Tax ID format for country code BR.", exception.Message);
    }
}
