using System.Text.Json;
using System.Text.Json.Serialization;
using MachineryCRM.Domain.DomainServices;

namespace MachineryCRM.Domain.ValueObjects;

[JsonConverter(typeof(TaxIdJsonConverter))]
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

public class TaxIdJsonConverter : JsonConverter<TaxId>
{
    public override TaxId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        string value = string.Empty;
        string countryCode = "BR"; // Fallback padrão

        // Busca as propriedades de forma flexível (suporta camelCase e PascalCase)
        if (root.TryGetProperty("value", out var valProp) || root.TryGetProperty("Value", out valProp))
        {
            value = valProp.GetString() ?? "";
        }

        if (root.TryGetProperty("countryCode", out var ccProp) || root.TryGetProperty("CountryCode", out ccProp))
        {
            countryCode = ccProp.GetString() ?? "BR";
        }

        if (string.IsNullOrWhiteSpace(value)) return null;

        return new TaxId(value, countryCode);
    }

    public override void Write(Utf8JsonWriter writer, TaxId value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("value", value.Value);
        writer.WriteEndObject();
    }
}