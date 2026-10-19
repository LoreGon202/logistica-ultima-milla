namespace Logistica.Application;

public interface ICourierAvailabilityChecker
{
    // Responde si el repartidor puede recibir una nueva entrega
    // (por ejemplo, si su ruta actual no está saturada).
    Task<bool> IsAvailableAsync(Guid courierId);
}
