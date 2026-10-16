namespace MachineryCRM.Domain.Entities;

public class Machine : Entity
{

    public string? SerialNumber { get; private set; }
    public DateTime ManufactureDate { get; private set; }
    
    // Vínculo com o modelo padrão (Engenharia)
    public Guid MachineModelId { get; private set; }
    public Item MachineModel { get; private set; } 

    // Vínculos com o ecossistema CRM existente
    public Guid? CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    
    public Guid? SiteId { get; private set; }
    public Site Site { get; private set; }

    public string Model { get; private set; } = null!;
    public string? Brand { get; private set; }
    public string? Status { get; private set; }
    public DateTime AcquisitionDate { get; private set; }

    public ICollection<TransferHistory> TransferHistories { get; private set; } = new List<TransferHistory>();
    public ICollection<Maintenance> Maintenances { get; private set; } = new List<Maintenance>();
    public ICollection<Communication> Communications { get; private set; } = new List<Communication>();

    private readonly List<AssetComponent> _assetComponents = new();
    public IReadOnlyCollection<AssetComponent> AssetComponents => _assetComponents.AsReadOnly();

    public Machine(Guid siteId, string serialNumber, string model, string? brand, string? status, DateTime acquisitionDate)
    {
        SiteId = siteId;
        
        ArgumentException.ThrowIfNullOrWhiteSpace(serialNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        SerialNumber = serialNumber;
        Model = model;
        Brand = brand;
        Status = status;
        AcquisitionDate = acquisitionDate;
    }

    public void AssignToCustomer(Guid customerId, Guid siteId)
    {
        CustomerId = customerId;
        SiteId = siteId;
    }

    public void AddPhysicalComponent(AssetComponent component)
    {
        _assetComponents.Add(component);
    }
}