using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Api.IntegrationTests;

// Cada prueba arranca la API real con su propia base de datos SQLite temporal,
// creada por las mismas migraciones que usa la aplicación.
public sealed class TestApiFactory : WebApplicationFactory<Program>
{
    private readonly string _dbPath =
        Path.Combine(Path.GetTempPath(), $"logistica-test-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Logistica", $"Data Source={_dbPath}");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        SqliteConnection.ClearAllPools();
        try { File.Delete(_dbPath); } catch (IOException) { }
    }
}

public class AssignCourierEndpointTests
{
    private const string DeliveryA = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
    private const string DeliveryB = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb";
    private static readonly Guid CourierAvailable = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid CourierSaturated = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public sealed record DeliveryDto(Guid DeliveryId, Guid? CourierId, string Estado);
    public sealed record ErrorDto(string Code, string Message);

    // Escenario 1: asignación exitosa, y el dato queda guardado en la base de datos
    [Fact]
    public async Task Asignar_repartidor_disponible_responde_200_y_persiste()
    {
        using var factory = new TestApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryA}/assign-courier", new { courierId = CourierAvailable });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var consulta = await client.GetFromJsonAsync<DeliveryDto>($"/deliveries/{DeliveryA}");
        Assert.NotNull(consulta);
        Assert.Equal("Asignada", consulta!.Estado);
        Assert.Equal(CourierAvailable, consulta.CourierId);
    }

    // Escenario 3: invariante, una entrega no puede asignarse dos veces
    [Fact]
    public async Task Asignar_dos_veces_la_misma_entrega_responde_409()
    {
        using var factory = new TestApiFactory();
        using var client = factory.CreateClient();

        await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryA}/assign-courier", new { courierId = CourierAvailable });
        var segunda = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryA}/assign-courier", new { courierId = CourierAvailable });

        Assert.Equal(HttpStatusCode.Conflict, segunda.StatusCode);
        var error = await segunda.Content.ReadFromJsonAsync<ErrorDto>();
        Assert.Equal("DELIVERY_ALREADY_ASSIGNED", error!.Code);
    }

    // Escenario 4: caso límite, repartidor con ruta saturada
    [Fact]
    public async Task Asignar_repartidor_saturado_responde_409_y_no_cambia_la_entrega()
    {
        using var factory = new TestApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryB}/assign-courier", new { courierId = CourierSaturated });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorDto>();
        Assert.Equal("COURIER_NOT_AVAILABLE", error!.Code);

        var consulta = await client.GetFromJsonAsync<DeliveryDto>($"/deliveries/{DeliveryB}");
        Assert.Equal("Pendiente", consulta!.Estado);
        Assert.Null(consulta.CourierId);
    }

    // Escenario 2: la entrega no existe
    [Fact]
    public async Task Asignar_entrega_inexistente_responde_404()
    {
        using var factory = new TestApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{Guid.NewGuid()}/assign-courier", new { courierId = CourierAvailable });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorDto>();
        Assert.Equal("DELIVERY_NOT_FOUND", error!.Code);
    }

    // Validación de la entrada: el identificador del repartidor es obligatorio
    [Fact]
    public async Task Asignar_con_repartidor_vacio_responde_400()
    {
        using var factory = new TestApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryA}/assign-courier", new { courierId = Guid.Empty });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}