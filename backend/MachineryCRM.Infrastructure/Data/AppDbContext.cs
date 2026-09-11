using MachineryCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MachineryCRM.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<MaintenanceOrder> MaintenanceOrders => Set<MaintenanceOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Enables PostGIS and pgvector extensions as defined in the architectural requirements
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.HasPostgresExtension("vector");

        // Automatically applies all IEntityTypeConfiguration classes found in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}