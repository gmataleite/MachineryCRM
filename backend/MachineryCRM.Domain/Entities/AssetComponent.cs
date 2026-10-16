namespace MachineryCRM.Domain.Entities;

public class AssetComponent : Entity
{
    public Guid MachineId { get; private set; }
    public Machine Machine { get; private set; }

    // Autorelacionamento físico para espelhar a submontagem exata no campo
    public Guid? ParentAssetComponentId { get; private set; }
    public AssetComponent ParentAssetComponent { get; private set; }

    // Referência de qual peça é essa
    public Guid ItemId { get; private set; }
    public Item Item { get; private set; }

    public decimal Quantity { get; private set; }
    public string SerialNumber { get; private set; } // Caso a peça/motor exija rastreio próprio
    
    public DateTime InstalledAt { get; private set; }
    public DateTime? RemovedAt { get; private set; }

    private readonly List<AssetComponent> _childComponents = new();
    public IReadOnlyCollection<AssetComponent> ChildComponents => _childComponents.AsReadOnly();

    protected AssetComponent() { }

    public AssetComponent(Guid machineId, Guid itemId, decimal quantity, Guid? parentAssetComponentId = null, string serialNumber = null)
    {
        MachineId = machineId;
        ItemId = itemId;
        Quantity = quantity;
        ParentAssetComponentId = parentAssetComponentId;
        SerialNumber = serialNumber;
        InstalledAt = DateTime.UtcNow;
    }

    // Comportamento: Baixa da peça (Retrofit)
    public void MarkAsRemoved()
    {
        if (RemovedAt.HasValue)
            throw new InvalidOperationException("Este componente já foi removido.");
            
        RemovedAt = DateTime.UtcNow;
    }
}