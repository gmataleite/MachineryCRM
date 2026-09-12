namespace MachineryCRM.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}