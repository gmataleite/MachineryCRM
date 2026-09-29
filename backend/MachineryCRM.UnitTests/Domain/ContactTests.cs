using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Enums;
using Xunit;

namespace MachineryCRM.UnitTests.Domain;

public class ContactTests
{
    [Theory]
    [InlineData("John Doe", "1234567890", "john.doe@example.com")]
    [InlineData("John Doe", null, "john.doe@example.com")]
    [InlineData("John Doe", "1234567890", null)]
    [InlineData("John Doe", null, null)]

    public void CreateContact_ShouldInitializePropertiesCorrectly(string name, string? phone, string? email)
    {
        // Act
        var contact = new Contact(Guid.NewGuid(), name, phone, email);

        // Assert
        Assert.Equal(name, contact.Name);
        Assert.Equal(email, contact.Email);
        Assert.Equal(phone, contact.Phone);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var contact = new Contact(Guid.NewGuid(), "John Doe", "1234567890", "john.doe@example.com");

        // Act
        contact.UpdateDetails("Jane Doe", "0987654321", "jane.doe@example.com", "Updated observations");

        // Assert
        Assert.Equal("Jane Doe", contact.Name);
        Assert.Equal("jane.doe@example.com", contact.Email);
        Assert.Equal("0987654321", contact.Phone);
        Assert.Equal("Updated observations", contact.Observations);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("john.doe@")]
    [InlineData("@example.com")]
    [InlineData("john.doe@example")]
    public void CreateContact_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        // Arrange
        var name = "John Doe";
        var phone = "1234567890";
        var email = invalidEmail;

        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => new Contact(Guid.NewGuid(), name, phone, email));
        Assert.Equal("Invalid email format.", exception.Message);
    }

    [Theory]
    [InlineData("invalid-phone")]
    [InlineData("phone123")]
    [InlineData("!@#$%^&*()")]
    [InlineData("123-456-7890")]
    [InlineData("1234567890101234567890")]
    public void CreateContact_WithInvalidPhone_ShouldThrowArgumentException(string invalidPhone)
    {
        // Arrange
        var name = "John Doe";
        var phone = invalidPhone;
        var email = "john.doe@example.com";

        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => new Contact(Guid.NewGuid(), name, phone, email));
        Assert.Equal("Invalid phone number format.", exception.Message);
    } 
}