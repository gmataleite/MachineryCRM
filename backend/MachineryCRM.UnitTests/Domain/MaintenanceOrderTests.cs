using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Enums;
using Xunit;

namespace MachineryCRM.UnitTests.Domain;

public class MaintenanceOrderTests
{
    [Fact]
    public void StartMaintenance_ShouldChangeStatusToInProgress_WhenPending()
    {
        // Arrange
        var order = new MaintenanceOrder(Guid.NewGuid(), Guid.NewGuid(), "Hydraulic fluid leak", DateTime.UtcNow);

        // Act
        order.StartMaintenance();

        // Assert
        Assert.Equal(MaintenanceStatus.InProgress, order.Status);
        Assert.NotNull(order.UpdatedAt);
    }

    [Fact]
    public void CompleteMaintenance_ShouldThrowException_WhenNotInProgress()
    {
        // Arrange
        var order = new MaintenanceOrder(Guid.NewGuid(), Guid.NewGuid(), "Hydraulic fluid leak", DateTime.UtcNow);

        // Act
        Action act = () => order.CompleteMaintenance();

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(act);
        Assert.Equal("Only in-progress orders can be completed.", exception.Message);
    }
}