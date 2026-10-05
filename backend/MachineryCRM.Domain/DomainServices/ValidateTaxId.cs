using MachineryCRM.Domain.Interfaces;

namespace MachineryCRM.Domain.DomainServices;

public static class ValidateTaxId
{
    private static readonly Dictionary<string, ITaxIDValidator> Validators = new()
    {
        { "BR", new BrTaxValidator() }
        // Add other country validators here
    };

    public static bool IsValid(string countryCode, string? taxId)
    {
        if (string.IsNullOrWhiteSpace(taxId))
            return true; // Assuming empty TaxId is valid

        if (Validators.TryGetValue(countryCode.ToUpper(), out var validator))
        {
            return validator.IsValid(taxId);
        }

        // If no specific validator is found for the country, consider it valid
        return true;
    }
}