using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MachineryCRM.Infrastructure.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Garante que o banco existe e as migrations foram aplicadas
        context.Database.Migrate();

        // Aborta o seeding se já existirem clientes no banco
        if (context.Customers.Any())
            return;

        var tech1 = new AppUser("Roberto Almeida", "roberto.almeida@example.com", "hash_temporario_123", "Technician");
        var tech2 = new AppUser("Carla Mendes", "carla.mendes@example.com", "hash_temporario_456", "Technician");
        
        context.AppUsers.AddRange(tech1, tech2);
        context.SaveChanges();

        var customer = new Customer("AgroTech Solutions");
        context.Customers.Add(customer);
        context.SaveChanges();

        var site1 = new Site(customer.Id, "Sede Fazenda Bela Vista", null);
        context.Sites.Add(site1);
        context.SaveChanges();

        var machine1 = new Machine(site1.Id, "SN-77489-XYZ", "Colheitadeira AX-900", "AgroMax", "Ativa", DateTime.UtcNow.AddYears(-2));
        var machine2 = new Machine(site1.Id, "SN-11200-ABC", "Trator de Esteira T-50", "TerraTech", "Ativa", DateTime.UtcNow.AddYears(-3));
        var machine3 = new Machine(site1.Id, "SN-99882-QWE", "Pulverizador Autopropelido P-300", "AgroMax", "Ativa", DateTime.UtcNow.AddYears(-1));

        context.Machines.AddRange(machine1, machine2, machine3);
        context.SaveChanges();

        var order1 = new Maintenance(
            machine1.Id, 
            tech1.Id, 
            DateTime.UtcNow.AddDays(2),
            "Corretiva: Vazamento no sistema hidráulico primário.", 
            "O-rings e fluidos"
        );

        var order2 = new Maintenance(
            machine2.Id, 
            tech2.Id, 
            DateTime.UtcNow.AddDays(5),
            "Preventiva: Revisão de 1000 horas.", 
            "Filtros de óleo"
        );

        // Transição de estado encapsulada testando a regra de domínio
        order2.StartMaintenance(); 

        context.Maintenances.AddRange(order1, order2);
        context.SaveChanges();
    }
}