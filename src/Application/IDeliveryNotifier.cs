using Logistica.Domain;

namespace Logistica.Application;

public interface IDeliveryNotifier
{
    Task NotifyCourierAssignedAsync(DeliveryId deliveryId, Guid courierId);
}
