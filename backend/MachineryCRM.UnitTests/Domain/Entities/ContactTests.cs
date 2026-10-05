using MachineryCRM.Domain.Entities;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.Entities;

public class ContactTests
{
    [Fact]
    public void CreateContact_ShouldInitializePropertiesCorrectly()
    {
        var customerId = Guid.NewGuid();
        var contact = new Contact(customerId, "John Doe", "1234567890", "john.doe@example.com", "Notes");

        Assert.Equal(customerId, contact.CustomerId);
        Assert.Equal("John Doe", contact.Name);
        Assert.Equal("1234567890", contact.Phone);
        Assert.Equal("john.doe@example.com", contact.Email);
        Assert.Equal("Notes", contact.Observations);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CreateContact_ShouldThrowArgumentException_WhenNameIsInvalid(string? name)
    {
        var exception = Assert.ThrowsAny<ArgumentException>(() => new Contact(Guid.NewGuid(), name!, null, null, null));
        Assert.Equal("name", exception.ParamName);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("john.doe@")]
    [InlineData("@example.com")]
    public void Email_ShouldThrowFormatException_WhenInvalid(string invalidEmail)
    {
        var exception = Assert.Throws<FormatException>(() => new Contact(Guid.NewGuid(), "Name", null, invalidEmail, null));
        Assert.Equal("Invalid email format.", exception.Message);
    }

    [Theory]
    [InlineData("invalid-phone")]
    [InlineData("123-456-7890")]
    public void Phone_ShouldThrowFormatException_WhenInvalid(string invalidPhone)
    {
        var exception = Assert.Throws<FormatException>(() => new Contact(Guid.NewGuid(), "Name", invalidPhone, null, null));
        Assert.Equal("Invalid phone number format.", exception.Message);
    } 

    [Fact]
    public void UpdateDetails_ShouldUpdateProperties()
    {
        var contact = new Contact(Guid.NewGuid(), "Old", "12345", "old@ex.com", "Old obs");
        
        contact.UpdateDetails("New", "98765", "new@ex.com", "New obs");

        Assert.Equal("New", contact.Name);
        Assert.Equal("98765", contact.Phone);
        Assert.Equal("new@ex.com", contact.Email);
        Assert.Equal("New obs", contact.Observations);
    }

    [Fact]
    public void ChangeSite_ShouldSetSiteIdAndClearFiscalEntityId()
    {
        var contact = new Contact(Guid.NewGuid(), "Name", null, null, null);
        contact.ChangeFiscalEntity(Guid.NewGuid());
        
        var siteId = Guid.NewGuid();
        contact.ChangeSite(siteId);

        Assert.Equal(siteId, contact.SiteId);
        Assert.Null(contact.FiscalEntityId);
    }

    [Fact]
    public void ChangeFiscalEntity_ShouldSetFiscalEntityIdAndClearSiteId()
    {
        var contact = new Contact(Guid.NewGuid(), "Name", null, null, null);
        contact.ChangeSite(Guid.NewGuid());

        var fiscalEntityId = Guid.NewGuid();
        contact.ChangeFiscalEntity(fiscalEntityId);

        Assert.Equal(fiscalEntityId, contact.FiscalEntityId);
        Assert.Null(contact.SiteId);
    }
}