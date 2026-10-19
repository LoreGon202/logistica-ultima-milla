using Logistica.Domain;
using Xunit;

namespace Domain.Tests;

public class DeliveryTests
{
    // Escenario 1: Asignación exitosa
    [Fact]
    public void AssignCourier_CuandoNoTieneRepartidor_AsignaCorrectamente()
    {
        // Dado
        var delivery = new Delivery(DeliveryId.New(), new DeliveryAddress("Calle 123"));
        var courierId = Guid.NewGuid();

        // Cuando
        delivery.AssignCourier(courierId);

        // Entonces
        Assert.Equal(courierId, delivery.CourierId);
        Assert.Equal(EstadoEntrega.Asignada, delivery.Estado);
    }

    // Escenario 3: Entrega ya asignada
    [Fact]
    public void AssignCourier_CuandoYaTieneRepartidor_LanzaExcepcion()
    {
        // Dado
        var delivery = new Delivery(DeliveryId.New(), new DeliveryAddress("Calle 123"));
        var primerCourier = Guid.NewGuid();
        delivery.AssignCourier(primerCourier);

        var segundoCourier = Guid.NewGuid();

        // Cuando / Entonces
        var exception = Assert.Throws<InvalidOperationException>(
            () => delivery.AssignCourier(segundoCourier));

        Assert.Equal("La entrega ya tiene un repartidor asignado.", exception.Message);
        // El repartidor original se mantiene sin cambios
        Assert.Equal(primerCourier, delivery.CourierId);
    }

    [Fact]
    public void DeliveryAddress_CuandoEsVacia_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() => new DeliveryAddress(""));
    }

    [Fact]
    public void Delivery_AlCrearse_IniciaEnEstadoPendiente()
    {
        var delivery = new Delivery(DeliveryId.New(), new DeliveryAddress("Calle 123"));

        Assert.Equal(EstadoEntrega.Pendiente, delivery.Estado);
        Assert.Null(delivery.CourierId);
    }
}
