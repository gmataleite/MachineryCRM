using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Services;
using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Enums;
using MachineryCRM.Domain.Interfaces;
using Moq;
using Xunit;

namespace MachineryCRM.UnitTests.Application.Services;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerService = new CustomerService(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task UpdateContactAsync_ShouldTransferContactToSite_WhenSiteIdIsUpdated()
    {
        var customerId = Guid.NewGuid();
        var contactId = Guid.NewGuid();
        var newSiteId = Guid.NewGuid();
        
        var contact = new Contact(customerId, "Old Name", "12345", "old@ex.com", "Obs");
        // O contato originalmente pertencia a uma entidade fiscal
        contact.ChangeFiscalEntity(Guid.NewGuid()); 

        var updateDto = new UpdateContactDto
        {
            Name = "New Name",
            SiteId = newSiteId, 
            FiscalEntityId = null,
            Phone = "12345",
            Email = "new@ex.com",
            Observations = "New Obs"
        };

        _customerRepositoryMock.Setup(repo => repo.GetContactByIdAsync(contactId)).ReturnsAsync(contact);

        await _customerService.UpdateContactAsync(contactId, updateDto);

        Assert.Equal(newSiteId, contact.SiteId);
        Assert.Null(contact.FiscalEntityId); 
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ReorderGeoPointsAsync_ShouldApplyReorderingAndCommit_WhenDataIsValid()
    {
        var siteId = Guid.NewGuid();
        var site = new Site(Guid.NewGuid(), "Farm", null);
        
        // Arrange: p1 inicia com hierarquia maior (1) e p2 com hierarquia menor (0)
        var p1 = new GeoPoint(siteId, "G1", 0, 0, 0, (GeoLocationType)1);
        var p2 = new GeoPoint(siteId, "G2", 0, 0, 0, (GeoLocationType)0);
        
        var p1Id = p1.Id;
        var p2Id = p2.Id;
        
        site.GeoPoints.Add(p1);
        site.GeoPoints.Add(p2);

        _customerRepositoryMock.Setup(repo => repo.GetSiteByIdWithGeoPointsAsync(siteId)).ReturnsAsync(site);

        // Act: Reordena colocando p2 (hierarquia 0) antes de p1 (hierarquia 1). Regra preservada.
        var dtos = new List<ReorderGeoPointDto>
        {
            new ReorderGeoPointDto { Id = p2Id, Order = 0 }, 
            new ReorderGeoPointDto { Id = p1Id, Order = 1 }
        };

        await _customerService.ReorderGeoPointsAsync(siteId, dtos);

        // Assert
        Assert.Equal(1, p1.Order);
        Assert.Equal(0, p2.Order);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ReorderGeoPointsAsync_ShouldThrowKeyNotFoundException_WhenSiteDoesNotExist()
    {
        var siteId = Guid.NewGuid();
        _customerRepositoryMock.Setup(repo => repo.GetSiteByIdWithGeoPointsAsync(siteId)).ReturnsAsync((Site?)null);

        var dtos = new List<ReorderGeoPointDto>();

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.ReorderGeoPointsAsync(siteId, dtos));
        Assert.Equal("Site não encontrado.", exception.Message);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task AddContactAsync_ShouldMapSiteCorrectly_WhenSiteIdIsProvided()
    {
        var customerId = Guid.NewGuid();
        var siteId = Guid.NewGuid();
        
        var dto = new CreateContactDto
        {
            Name = "John",
            SiteId = siteId,
            Phone = "12345",
            Email = "j@j.com"
        };

        _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(customerId)).ReturnsAsync(new Customer("Test"));

        var result = await _customerService.AddContactAsync(customerId, dto);

        Assert.NotNull(result);
        Assert.Equal(siteId, result.SiteId);
        _customerRepositoryMock.Verify(repo => repo.AddContact(It.Is<Contact>(c => c.SiteId == siteId)), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }
}