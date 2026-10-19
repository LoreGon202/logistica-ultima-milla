using Logistica.Domain;

namespace Logistica.Application;

public interface IDeliveryRepository
{
    Task<Delivery?> GetAsync(DeliveryId id);
    Task SaveAsync(Delivery delivery);
}
