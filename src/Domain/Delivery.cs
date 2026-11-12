namespace Logistica.Domain;

public enum EstadoEntrega
{
    Pendiente,
    Asignada,
    EnRuta,
    Entregada,
    ConIncidencia
}

public sealed class Delivery
{
    public DeliveryId Id { get; }
    public DeliveryAddress Address { get; }
    public Guid? CourierId { get; private set; }
    public EstadoEntrega Estado { get; private set; }

    public Delivery(DeliveryId id, DeliveryAddress address)
    {
        Id = id;
        Address = address;
        Estado = EstadoEntrega.Pendiente;
    }

    // Reconstruye una entrega ya existente (por ejemplo, leída de la base de datos)
    // sin saltarse el constructor ni exponer los setters.
    public static Delivery Restore(
        DeliveryId id, DeliveryAddress address, Guid? courierId, EstadoEntrega estado)
    {
        var delivery = new Delivery(id, address);
        delivery.CourierId = courierId;
        delivery.Estado = estado;
        return delivery;
    }

    // Invariante: una entrega no puede asignarse a un repartidor si ya tiene uno asignado.
    public void AssignCourier(Guid courierId)
    {
        if (CourierId.HasValue)
            throw new DeliveryAlreadyAssignedException();

        CourierId = courierId;
        Estado = EstadoEntrega.Asignada;
    }
}

public sealed class DeliveryAlreadyAssignedException : InvalidOperationException
{
    public DeliveryAlreadyAssignedException()
        : base("La entrega ya tiene un repartidor asignado.") { }
}