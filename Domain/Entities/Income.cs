using Domain.Enums;

namespace Domain.Entities;

// Representa un ingreso como raíz de agregado y garantiza las invariantes del caso de uso de registro de ingresos.
public sealed class Income
{
    private Income()
    {
        Description = string.Empty;
        Source = string.Empty;
    }

    private Income(decimal amount, string description, DateOnly receivedOn, string source, Currency currency)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description is required.", nameof(description));
        }

        if (string.IsNullOrWhiteSpace(source))
        {
            throw new ArgumentException("Source is required.", nameof(source));
        }

        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
        Description = description.Trim();
        ReceivedOn = receivedOn;
        Source = source.Trim();
        Currency = currency;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; }
    public DateOnly ReceivedOn { get; private set; }
    public string Source { get; private set; }
    public Currency Currency { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public static Income Create(decimal amount, string description, DateOnly receivedOn, string source, Currency currency)
        => new(amount, description, receivedOn, source, currency);
}