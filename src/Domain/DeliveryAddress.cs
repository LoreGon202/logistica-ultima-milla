namespace Logistica.Domain;

public sealed record DeliveryAddress(string Value)
{
    public string Value { get; } = !string.IsNullOrWhiteSpace(Value)
        ? Value
        : throw new ArgumentException("La dirección de entrega no puede estar vacía.");
}