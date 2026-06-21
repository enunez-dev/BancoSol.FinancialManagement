namespace Infrastructure.ExchangeRates;

// Modela la configuración necesaria para conectarse con el proveedor externo HexaRate.
public sealed class HexaRateOptions
{
    public const string SectionName = "HexaRate";

    public string BaseUrl { get; set; } = "https://hexarate.paikama.co/";
}