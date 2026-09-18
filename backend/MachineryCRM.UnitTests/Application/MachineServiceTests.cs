using FluentAssertions;
using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Services;
using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Interfaces;
using Moq;
using Xunit;

namespace MachineryCRM.UnitTests.Application;

public class MachineServiceTests
{
    private readonly Mock<IMachineRepository> _machineRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly MachineService _machineService;

    public MachineServiceTests()
    {
        _machineRepositoryMock = new Mock<IMachineRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _machineService = new MachineService(
            _machineRepositoryMock.Object, 
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenSerialNumberExists()
    {
        // Arrange
        Guid fakeSiteId = Guid.NewGuid();
        var dto = new CreateMachineDto(fakeSiteId, "SN-12345", "Trator T-50", 2024);
        var existingMachine = new Machine(dto.SiteId, dto.SerialNumber, dto.Model, dto.Year);
        
        _machineRepositoryMock.Setup(repo => repo.GetBySerialNumberAsync(dto.SerialNumber))
            .ReturnsAsync(existingMachine);

        // Act
        Func<Task> act = async () => await _machineService.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("A machine with this serial number already exists.");
        
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnMachineDto_WhenSuccessful()
    {
        // Arrange
        Guid fakeSiteId = Guid.NewGuid();
        var dto = new CreateMachineDto(fakeSiteId, "SN-99999", "Colheitadeira AX-900", 2024);
        
        _machineRepositoryMock.Setup(repo => repo.GetBySerialNumberAsync(dto.SerialNumber))
            .ReturnsAsync((Machine?)null);

        // Act
        var result = await _machineService.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.SerialNumber.Should().Be(dto.SerialNumber);
        result.SiteId.Should().Be(dto.SiteId); 
        
        _machineRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Machine>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }
}