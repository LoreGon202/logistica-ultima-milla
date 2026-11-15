using Logistica.Domain;

namespace Logistica.Application;

public sealed class DeliveryNotFoundException : Exception
{
    public DeliveryNotFoundException(DeliveryId id)
        : base($"La entrega {id.Value} no existe.") { }
}

public sealed class CourierNotAvailableException : Exception
{
    public CourierNotAvailableException(Guid courierId)
        : base($"El repartidor {courierId} no está disponible.") { }
}