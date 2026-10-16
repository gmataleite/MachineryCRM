namespace MachineryCRM.Domain.Entities;

public class BillOfMaterial : Entity
{
    public Guid ParentItemId { get; private set; }
    public Item ParentItem { get; private set; }

    public Guid ChildItemId { get; private set; }
    public Item ChildItem { get; private set; }

    public decimal Quantity { get; private set; }
    public string PositionDesignator { get; private set; }

    protected BillOfMaterial() { }

    internal BillOfMaterial(Guid parentItemId, Guid childItemId, decimal quantity, string positionDesignator)
    {
        ParentItemId = parentItemId;
        ChildItemId = childItemId;
        Quantity = quantity;
        PositionDesignator = positionDesignator;
    }
}