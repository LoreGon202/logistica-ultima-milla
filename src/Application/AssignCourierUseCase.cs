using Logistica.Domain;

namespace Logistica.Application;

public sealed class AssignCourierUseCase
{
    private readonly IDeliveryRepository _repository;
    private readonly ICourierAvailabilityChecker _availabilityChecker;
    private readonly IDeliveryNotifier _notifier;

    public AssignCourierUseCase(
        IDeliveryRepository repository,
        ICourierAvailabilityChecker availabilityChecker,
        IDeliveryNotifier notifier)
    {
        _repository = repository;
        _availabilityChecker = availabilityChecker;
        _notifier = notifier;
    }

    public async Task ExecuteAsync(DeliveryId deliveryId, Guid courierId)
    {
        // 1. Cargar la entrega
        var delivery = await _repository.GetAsync(deliveryId)
            ?? throw new InvalidOperationException("La entrega no existe.");

        // 2. Consultar disponibilidad mediante un puerto, no una API concreta
        var disponible = await _availabilityChecker.IsAvailableAsync(courierId);
        if (!disponible)
            throw new InvalidOperationException("El repartidor no está disponible.");

        // 3. Proteger la invariante dentro de la entidad
        delivery.AssignCourier(courierId);

        // 4. Guardar
        await _repository.SaveAsync(delivery);

        // 5. Registrar el resultado
        await _notifier.NotifyCourierAssignedAsync(deliveryId, courierId);
    }
}
