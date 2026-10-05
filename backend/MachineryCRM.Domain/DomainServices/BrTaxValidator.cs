using MachineryCRM.Domain.Interfaces;

namespace MachineryCRM.Domain.DomainServices;

public class BrTaxValidator : ITaxIDValidator
{
    public string CountryCode => "BR";

    public bool IsValid(string taxId)
    {
        if (string.IsNullOrWhiteSpace(taxId)) return false;

        if (taxId.Any(c => !char.IsLetterOrDigit(c) && c != '.' && c != '-' && c != '/'))
            return false;

        var cleanedTaxId = new string(taxId.Where(char.IsLetterOrDigit).ToArray()).ToUpper();

        if (cleanedTaxId.Length == 11 && cleanedTaxId.All(char.IsDigit))
            return IsValidCPF(cleanedTaxId);
        
        if (cleanedTaxId.Length == 14)
            return IsValidCNPJ(cleanedTaxId);
        
        return false;
    }

    private static bool IsValidCPF(string cpf)
    {
        if (cpf.Distinct().Count() == 1) 
            return false;

        int sum1 = 0;
        int sum2 = 0;

        for (int i = 0; i < 9; i++)
        {
            int digit = cpf[i] - '0';
            sum1 += digit * (10 - i);
            sum2 += digit * (11 - i);
        }

        int remainder1 = (sum1 * 10) % 11;
        if (remainder1 == 10) remainder1 = 0;
        
        if (remainder1 != (cpf[9] - '0')) 
            return false;

        sum2 += remainder1 * 2;
        int remainder2 = (sum2 * 10) % 11;
        if (remainder2 == 10) remainder2 = 0;

        return remainder2 == (cpf[10] - '0');
    }

    private static bool IsValidCNPJ(string cnpj)
    {
        if (cnpj.Distinct().Count() == 1) 
            return false;

        // The last two characters (Check Digits) must be numeric
        if (!char.IsDigit(cnpj[12]) || !char.IsDigit(cnpj[13])) 
            return false;

        // Weights for DV1
        int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        // Weights for DV2[cite: 10]
        int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        int sum1 = 0;
        for (int i = 0; i < 12; i++)
        {
            // Subtract 48 from the character's ASCII decimal value
            // Multiply the first 12 values ​​obtained by the specific weights
            int numericValue = cnpj[i] - 48;
            sum1 += numericValue * multiplier1[i];
        }

        // Divide the total sum by 11 to find the remainder
        int remainder1 = sum1 % 11;
        // If the remainder is 0 or 1, the digit is 0; if it is between 2 and 10, the digit is 11 minus the remainder
        int digit1 = remainder1 < 2 ? 0 : 11 - remainder1;

        if (digit1 != (cnpj[12] - '0')) 
            return false;

        int sum2 = 0;
        for (int i = 0; i < 13; i++)
        {
            // Include the value of DV1 in the calculation of DV2, multiplying by the weights
            int numericValue = cnpj[i] - 48;
            sum2 += numericValue * multiplier2[i];
        }

        // Divide the new sum by 11 and get the remainder
        int remainder2 = sum2 % 11;
        // If the remainder is 0 or 1, the second digit will be 0; if it is between 2 and 10, the digit will be 11 minus the remainder
        int digit2 = remainder2 < 2 ? 0 : 11 - remainder2;

        return digit2 == (cnpj[13] - '0');
    }
}