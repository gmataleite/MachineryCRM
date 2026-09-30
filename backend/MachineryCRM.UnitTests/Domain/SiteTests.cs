using System;
using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Enums;

namespace MyApp;
class Program
{
    public void Main()
    {   
        var fazenda = new Site(Guid.NewGuid(), "Fazenda", "Bauru", "SP", "BR");

        var portaria = new GeoPoint(fazenda.Id, "portaria", 15.123, 20.456, GeoLocationType.Office, 2);
        var passagem = new GeoPoint(fazenda.Id, "passagem", 15.123, 20.456, GeoLocationType.Office, 1);
        var escritorio = new GeoPoint(fazenda.Id, "escritorio", 20.123, 45.456, GeoLocationType.Office, null);
        var maquina = new GeoPoint(fazenda.Id, "maquina", 10.123, 15.456, GeoLocationType.MachineLocation, null);

        Console.Write(fazenda.GeoPoints);
    }
}