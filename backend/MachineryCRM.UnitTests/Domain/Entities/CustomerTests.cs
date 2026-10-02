using MachineryCRM.Domain.Entities;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.Entities;

public class CustomerTests
{
    [Fact]
    public void CreateCustomer_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        string name = "John Doe";
        var customer = new Customer(name);

        // Assert
        Assert.Equal(name, customer.Name);
        Assert.NotEqual(Guid.Empty, customer.Id);
    }

    [Fact]
    public void UpdateCustomer_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var customer = new Customer("John Doe");

        // Act
        string newName = "Jane Doe";
        customer.UpdateName(newName);
        
        // Assert
        Assert.Equal(newName, customer.Name);       
    }
        
    [Fact]
    public void CreateCustomer_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Customer(""));
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void UpdateCustomer_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var customer = new Customer("John Doe");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => customer.UpdateName(""));
        Assert.Equal("name", exception.ParamName);
    }
}