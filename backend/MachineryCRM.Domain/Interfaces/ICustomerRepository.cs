using MachineryCRM.Domain.Entities;

namespace MachineryCRM.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    // Método específico para carregar o agregado completo (Cliente + Filhos)
    Task<Customer?> GetCustomerWithDetailsAsync(Guid id);
}