using MachineryCRM.Domain.Entities;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.Entities;

public class CustomerTests
{
    [Fact]
    public void CreateCustomer_ShouldInitializePropertiesCorrectly()
    {
        var name = "John Doe";
        var customer = new Customer(name);

        Assert.Equal(name, customer.Name);
        Assert.NotEqual(Guid.Empty, customer.Id);
    }
        
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CreateCustomer_ShouldThrowArgumentException_WhenNameIsInvalid(string? name)
    {
        var exception = Assert.ThrowsAny<ArgumentException>(() => new Customer(name!));
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateName()
    {
        var customer = new Customer("Old Name");
        customer.UpdateDetails("New Name");

        Assert.Equal("New Name", customer.Name);
    }
}