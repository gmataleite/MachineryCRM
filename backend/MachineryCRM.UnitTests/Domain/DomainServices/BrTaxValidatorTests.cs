using MachineryCRM.Domain.DomainServices;
using Xunit;

namespace MachineryCRM.UnitTests.Domain.DomainServices;

public class BrTaxValidatorTests
{
    private readonly BrTaxValidator _validator;

    public BrTaxValidatorTests()
    {
        _validator = new BrTaxValidator();
    }

    [Fact]
    public void CountryCode_ShouldBeBR()
    {
        Assert.Equal("BR", _validator.CountryCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void IsValid_ShouldReturnFalse_WhenNullOrWhitespace(string? taxId)
    {
        Assert.False(_validator.IsValid(taxId!));
    }

    [Theory]
    [InlineData("49938200036")] // CPF sem máscara
    [InlineData("499.382.000-36")] // CPF com máscara
    [InlineData("85150611000179")] // CNPJ sem máscara
    [InlineData("85.150.611/0001-79")] // CNPJ com máscara
    [InlineData("12ABC345000188")] // CNPJ alfanumérico sem máscara
    [InlineData("12.ABC.345/0001-88")] // CNPJ alfanumérico com máscara
    public void IsValid_ShouldReturnTrue_ForValidCpfAndCnpj(string validTaxId)
    {
        Assert.True(_validator.IsValid(validTaxId));
    }

    [Theory]
    [InlineData("49938200037")] // CPF: dígito verificador incorreto
    [InlineData("85150611000178")] // CNPJ: dígito verificador incorreto
    [InlineData("12ABC345000189")] // CNPJ alfanumérico: dígito verificador incorreto
    public void IsValid_ShouldReturnFalse_WhenCheckDigitsAreInvalid(string invalidTaxId)
    {
        Assert.False(_validator.IsValid(invalidTaxId));
    }

    [Theory]
    [InlineData("11111111111")] // CPF com todos os dígitos iguais
    [InlineData("00000000000000")] // CNPJ numérico com todos os dígitos iguais
    [InlineData("AAAAAAAAAAAAAA")] // CNPJ alfanumérico com todos os caracteres iguais
    public void IsValid_ShouldReturnFalse_WhenAllCharactersAreTheSame(string repeatedCharsTaxId)
    {
        Assert.False(_validator.IsValid(repeatedCharsTaxId));
    }

    [Theory]
    [InlineData("49938200036!")] // Caractere especial não permitido
    [InlineData("851506110001_79")] // Underscore não permitido
    [InlineData("12345")] // Comprimento inválido após limpeza (5)
    [InlineData("1234567890123")] // Comprimento inválido após limpeza (13)
    [InlineData("123456789012345")] // Comprimento inválido após limpeza (15)
    public void IsValid_ShouldReturnFalse_WhenFormatOrLengthIsInvalid(string invalidFormatOrLength)
    {
        Assert.False(_validator.IsValid(invalidFormatOrLength));
    }

    [Theory]
    [InlineData("499A8200036")] // CPF contendo letra (deve ser rejeitado, pois CPF exige All(char.IsDigit))
    public void IsValid_ShouldReturnFalse_WhenCpfContainsLetters(string alphanumericCpf)
    {
        Assert.False(_validator.IsValid(alphanumericCpf));
    }

    [Theory]
    [InlineData("12ABC3450001AB")] // CNPJ: os últimos dois caracteres devem ser estritamente numéricos
    public void IsValid_ShouldReturnFalse_WhenCnpjCheckDigitsAreNotNumbers(string cnpjWithAlphaCheckDigits)
    {
        Assert.False(_validator.IsValid(cnpjWithAlphaCheckDigits));
    }
}