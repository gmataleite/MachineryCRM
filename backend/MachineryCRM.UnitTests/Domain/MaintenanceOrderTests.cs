using FluentAssertions;
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
        order.Status.Should().Be(MaintenanceStatus.InProgress);
        order.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void CompleteMaintenance_ShouldThrowException_WhenNotInProgress()
    {
        // Arrange
        var order = new MaintenanceOrder(Guid.NewGuid(), Guid.NewGuid(), "Hydraulic fluid leak", DateTime.UtcNow);

        // Act
        Action act = () => order.CompleteMaintenance();

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Only in-progress orders can be completed.");
    }
}