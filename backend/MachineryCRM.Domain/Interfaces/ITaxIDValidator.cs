namespace MachineryCRM.Domain.Interfaces;

public interface ITaxIDValidator
{
    public string CountryCode { get; }
    public bool IsValid(string taxId);
}