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
        var fakeSiteId = Guid.NewGuid();
        var dto = new CreateMachineDto
        {
            SiteId = fakeSiteId,
            SerialNumber = "SN-12345",
            Model = "Trator T-50",
            Brand = "AgroTech",
            Status = "Ativo",
            AcquisitionDate = DateTime.UtcNow.Date
        };
        
        var existingMachine = new Machine(dto.SiteId, dto.SerialNumber, dto.Model, dto.Brand, dto.Status, dto.AcquisitionDate);
        
        _machineRepositoryMock.Setup(repo => repo.GetBySerialNumberAsync(dto.SerialNumber))
            .ReturnsAsync(existingMachine);

        // Act
        Func<Task> act = async () => await _machineService.CreateAsync(dto);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Equal("A machine with this serial number already exists.", exception.Message);
        
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnMachineDto_WhenSuccessful()
    {
        // Arrange
        var fakeSiteId = Guid.NewGuid();
        var dto = new CreateMachineDto
        {
            SiteId = fakeSiteId,
            SerialNumber = "SN-99999",
            Model = "Colheitadeira AX-900",
            Brand = "AgroMax",
            Status = "Novo",
            AcquisitionDate = DateTime.UtcNow.Date
        };
        
        _machineRepositoryMock.Setup(repo => repo.GetBySerialNumberAsync(dto.SerialNumber))
            .ReturnsAsync((Machine?)null);

        // Act
        var result = await _machineService.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.SerialNumber, result.SerialNumber);
        Assert.Equal(dto.SiteId, result.SiteId);
        Assert.Equal(dto.Brand, result.Brand);
        Assert.Equal(dto.Status, result.Status);
        Assert.Equal(dto.AcquisitionDate, result.AcquisitionDate);
        
        _machineRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Machine>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }
}