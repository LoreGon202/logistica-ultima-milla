using Logistica.Application;
using Logistica.Domain;

namespace Logistica.Infrastructure;

public sealed class LoggingDeliveryNotifier : IDeliveryNotifier
{
    public Task NotifyCourierAssignedAsync(DeliveryId deliveryId, Guid courierId)
    {
        Console.WriteLine($"[Entrega {deliveryId.Value}] Repartidor {courierId} asignado correctamente.");
        return Task.CompletedTask;
    }
}
