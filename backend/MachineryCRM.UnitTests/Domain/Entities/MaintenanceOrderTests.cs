using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Enums;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.Entities;

public class MaintenanceTests
{
    [Fact]
    public void StartMaintenance_ShouldChangeStatusToInProgress_WhenPending()
    {
        // Arrange
        var order = new Maintenance(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Hydraulic fluid leak", "Orings, hydraulic fluid");

        // Act
        order.StartMaintenance();

        // Assert
        Assert.Equal(MaintenanceStatus.InProgress, order.Status);
        Assert.NotNull(order.UpdatedAt);
    }

    [Fact]
    public void CompleteMaintenance_ShouldThrowException_WhenNotInProgress()
    {
        // Arrange (Corrigido a ordem e tipagem dos parâmetros)
        var order = new Maintenance(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Hydraulic fluid leak", null);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => order.CompleteMaintenance());
        Assert.Equal("Only in-progress orders can be completed.", exception.Message);
    }

    [Fact]
    public void CompleteMaintenance_ShouldChangeStatusToCompleted_WhenInProgress()
    {
        // Arrange
        var order = new Maintenance(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, "Preventive", "Filters");
        order.StartMaintenance(); // Coloca no estado InProgress

        // Act
        order.CompleteMaintenance();

        // Assert
        Assert.Equal(MaintenanceStatus.Completed, order.Status);
        Assert.NotNull(order.CompletionDate);
    }
}