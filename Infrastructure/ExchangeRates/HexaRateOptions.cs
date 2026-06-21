namespace Infrastructure.ExchangeRates;

// Modela la configuración necesaria para conectarse con el proveedor externo HexaRate usando una URL completa configurable por entorno.
public sealed class HexaRateOptions
{
    public const string SectionName = "HexaRate";

    public string UsdBobLatestUrl { get; set; } = string.Empty;
}
