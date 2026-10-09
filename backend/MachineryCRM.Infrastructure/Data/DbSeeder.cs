using MachineryCRM.Domain.Entities;
using MachineryCRM.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MachineryCRM.Infrastructure.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context, IConfiguration configuration)
    {
        // Garante que o banco existe e as migrations foram aplicadas
        context.Database.Migrate();

        EnsureInitialAdmin(context, configuration);
        RepairLegacyExampleUsers(context);

        // Aborta o seeding se já existirem clientes no banco
        if (context.Customers.Any())
            return;

        var passwordHasher = new Pbkdf2PasswordHasher();
        var tech1 = new AppUser("Roberto Almeida", "roberto.almeida@example.com", passwordHasher.Hash("tecnico123"), UserRole.Technician);
        var tech2 = new AppUser("Carla Mendes", "carla.mendes@example.com", passwordHasher.Hash("tecnico123"), UserRole.Technician);
        
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

    private static void EnsureInitialAdmin(AppDbContext context, IConfiguration configuration)
    {
        var name = configuration["INITIAL_ADMIN_NAME"] ?? configuration["InitialAdmin:Name"];
        var email = configuration["INITIAL_ADMIN_EMAIL"] ?? configuration["InitialAdmin:Email"];
        var password = configuration["INITIAL_ADMIN_PASSWORD"] ?? configuration["InitialAdmin:Password"];

        var anyConfigured = name is not null || email is not null || password is not null;
        if (!anyConfigured)
            return;

        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "INITIAL_ADMIN_NAME, INITIAL_ADMIN_EMAIL and INITIAL_ADMIN_PASSWORD must all be configured together.");
        }

        var normalizedEmail = email.Trim();
        if (context.AppUsers.Any(user => user.Email == normalizedEmail))
            return;

        var passwordHasher = new Pbkdf2PasswordHasher();
        var admin = new AppUser(
            name.Trim(),
            normalizedEmail,
            passwordHasher.Hash(password),
            UserRole.Admin);

        context.AppUsers.Add(admin);
        context.SaveChanges();
    }

    private static void RepairLegacyExampleUsers(AppDbContext context)
    {
        var passwordHasher = new Pbkdf2PasswordHasher();
        var legacyUsers = context.AppUsers
            .Where(user =>
                user.Email == "roberto.almeida@example.com" ||
                user.Email == "carla.mendes@example.com")
            .ToList();

        var changed = false;
        foreach (var user in legacyUsers)
        {
            if (!user.PasswordHash.StartsWith("hash_temporario_", StringComparison.Ordinal))
                continue;

            user.ForcePasswordChange(passwordHasher.Hash("tecnico123"));
            changed = true;
        }

        if (changed)
            context.SaveChanges();
    }
}