using MachineryCRM.Domain.DomainServices;

namespace MachineryCRM.Domain.ValueObjects;

public record TaxId
{
    public string Value { get; }

    public TaxId(string value, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Tax ID cannot be null or whitespace.");
        }

        if (!ValidateTaxId.IsValid(countryCode, value))
        {
            throw new ArgumentException($"Invalid Tax ID format for country code {countryCode}.");
        }

        Value = value;
    }

    public override string ToString() => Value;

    // Permite que o Value Object seja tratado como string de forma transparente quando necessário
    public static implicit operator string?(TaxId? taxId) => taxId?.Value;
}