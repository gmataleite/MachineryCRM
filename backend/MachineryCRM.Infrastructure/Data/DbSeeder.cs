using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MachineryCRM.Infrastructure.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Garante que o banco existe e as migrations foram aplicadas
        context.Database.Migrate();

        // Aborta o seeding se já existirem máquinas no banco
        if (context.Machines.Any())
            return;

        var tech1 = new Technician("Roberto Almeida", "roberto.almeida@example.com", "CREA-10293");
        var tech2 = new Technician("Carla Mendes", "carla.mendes@example.com", "CREA-94821");
        
        context.Technicians.AddRange(tech1, tech2);
        context.SaveChanges(); // Salva para gerar os GUIDs

        var machine1 = new Machine("SN-77489-XYZ", "Colheitadeira AX-900", 2023);
        var machine2 = new Machine("SN-11200-ABC", "Trator de Esteira T-50", 2021);
        var machine3 = new Machine("SN-99882-QWE", "Pulverizador Autopropelido P-300", 2024);

        context.Machines.AddRange(machine1, machine2, machine3);
        context.SaveChanges();

        var order1 = new MaintenanceOrder(
            machine1.Id, 
            tech1.Id, 
            "Vazamento no sistema hidráulico primário.", 
            DateTime.UtcNow.AddDays(2));

        var order2 = new MaintenanceOrder(
            machine2.Id, 
            tech2.Id, 
            "Revisão preventiva de 1000 horas.", 
            DateTime.UtcNow.AddDays(5));

        // Transição de estado encapsulada testando a regra de domínio
        order2.StartMaintenance(); 

        context.MaintenanceOrders.AddRange(order1, order2);
        context.SaveChanges();
    }
}