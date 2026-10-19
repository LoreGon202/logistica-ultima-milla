using Logistica.Application;

namespace Logistica.Infrastructure;

public sealed class CourierServiceAvailabilityChecker : ICourierAvailabilityChecker
{
    private readonly HashSet<Guid> _repartidoresSaturados;

    public CourierServiceAvailabilityChecker(IEnumerable<Guid>? repartidoresSaturados = null)
    {
        _repartidoresSaturados = repartidoresSaturados is null
            ? new HashSet<Guid>()
            : new HashSet<Guid>(repartidoresSaturados);
    }

    // Implementación simple para esta actividad: un repartidor está disponible
    // si no está en la lista de repartidores con ruta saturada.
    // En la Actividad 4 esto se reemplazará por una consulta real al módulo de Rutas.
    public Task<bool> IsAvailableAsync(Guid courierId)
    {
        var disponible = !_repartidoresSaturados.Contains(courierId);
        return Task.FromResult(disponible);
    }
}
