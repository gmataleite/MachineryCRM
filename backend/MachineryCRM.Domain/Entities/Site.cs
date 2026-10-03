using MachineryCRM.Domain.ValueObjects;
using MachineryCRM.Domain.Enums;

namespace MachineryCRM.Domain.Entities;

public class Site : Entity
{
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; } = null!;
    public Address? Address { get; private set; }
    public ICollection<GeoPoint> GeoPoints { get; private set; } = new List<GeoPoint>();
    public ICollection<Contact> Contacts { get; private set; } = new List<Contact>();
    public ICollection<Machine> Machines { get; private set; } = new List<Machine>();

    public Site(Guid customerId, string name, Address? address)
    {
        CustomerId = customerId;

        UpdateDetails(name, address);
    }

    public void UpdateDetails(string name, Address? address)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Address = address;
    }

    public void ReorderGeoPoints(List<Guid> orderedIds)
    {
        ArgumentNullException.ThrowIfNull(orderedIds);

        if (orderedIds.Count != GeoPoints.Count)
            throw new InvalidOperationException("A lista de ordenação deve conter a exata quantidade de pontos deste site.");

        int previousTypeLevel = 0; // Inicia permitindo Office (0) ou superior

        for (int i = 0; i < orderedIds.Count; i++)
        {
            var point = GeoPoints.FirstOrDefault(p => p.Id == orderedIds[i]);
            
            if (point == null)
                throw new InvalidOperationException($"GeoPoint com ID {orderedIds[i]} não pertence a este Site.");

            // Regra de Domínio: A sequência não pode retroceder na hierarquia (ex: um Office após um Machine)
            int currentTypeLevel = (int)point.LocationType;
            
            if (currentTypeLevel < previousTypeLevel)
                throw new InvalidOperationException($"Ordem inválida. Um ponto do tipo {point.LocationType} não pode ser posicionado após um {(GeoLocationType)previousTypeLevel}.");

            previousTypeLevel = currentTypeLevel;
            
            point.ChangeOrder(i);
        }
    }
}
