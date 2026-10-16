using MachineryCRM.Domain.Enums;

namespace MachineryCRM.Domain.Entities;

public class Item : Entity
{
    public string Sku { get; private set; }
    public string Name { get; private set; }
    public string UnitOfMeasure { get; private set; }
    public ItemType Type { get; private set; }
    public decimal BaseCost { get; private set; }
    public bool IsActive { get; private set; }

    // EF Core Backing Fields para encapsulamento das coleções
    private readonly List<BillOfMaterial> _components = new();
    public IReadOnlyCollection<BillOfMaterial> Components => _components.AsReadOnly();

    private readonly List<BillOfMaterial> _usedIn = new();
    public IReadOnlyCollection<BillOfMaterial> UsedIn => _usedIn.AsReadOnly();

    protected Item() { }

    public Item(string sku, string name, string unitOfMeasure, ItemType type, decimal baseCost)
    {
        Sku = sku;
        Name = name;
        UnitOfMeasure = unitOfMeasure;
        Type = type;
        BaseCost = baseCost;
        IsActive = true;
    }

    // Comportamento de domínio para adicionar componentes à árvore
    public void AddComponent(Item childItem, decimal quantity, string positionDesignator = null)
    {
        if (Type == ItemType.Part)
            throw new InvalidOperationException("Uma peça (Part) não pode conter subcomponentes na engenharia.");

        var bom = new BillOfMaterial(this.Id, childItem.Id, quantity, positionDesignator);
        _components.Add(bom);
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}