using Logistica.Application;
using Logistica.Domain;

namespace Logistica.Infrastructure;

public sealed class InMemoryDeliveryRepository : IDeliveryRepository
{
    private readonly Dictionary<Guid, Delivery> _storage = new();

    public Task<Delivery?> GetAsync(DeliveryId id)
    {
        _storage.TryGetValue(id.Value, out var delivery);
        return Task.FromResult(delivery);
    }

    public Task SaveAsync(Delivery delivery)
    {
        _storage[delivery.Id.Value] = delivery;
        return Task.CompletedTask;
    }

    // Método auxiliar solo para pruebas y demostración.
    public void Seed(Delivery delivery) => _storage[delivery.Id.Value] = delivery;
}
