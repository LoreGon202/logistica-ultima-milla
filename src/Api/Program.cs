using Logistica.Application;
using Logistica.Domain;
using Logistica.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

static string GetConnectionString(IConfiguration configuration) =>
    configuration.GetConnectionString("Logistica") ?? "Data Source=logistica.db";

// Puertos -> adaptadores (la infraestructura se elige aquí, el dominio no la conoce)
builder.Services.AddSingleton<IDeliveryRepository>(sp =>
    new SqliteDeliveryRepository(GetConnectionString(sp.GetRequiredService<IConfiguration>())));
builder.Services.AddSingleton<ICourierAvailabilityChecker>(sp =>
    new SqliteCourierAvailabilityChecker(GetConnectionString(sp.GetRequiredService<IConfiguration>())));
builder.Services.AddSingleton<IDeliveryNotifier, LoggingDeliveryNotifier>();
builder.Services.AddTransient<AssignCourierUseCase>();

var app = builder.Build();

// Migraciones reproducibles: crea las tablas al arrancar si no existen
SqliteMigrator.Migrate(GetConnectionString(app.Configuration));

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/deliveries/{id:guid}", async (Guid id, IDeliveryRepository repository) =>
{
    var delivery = await repository.GetAsync(new DeliveryId(id));
    return delivery is null
        ? Results.NotFound(new ErrorResponse("DELIVERY_NOT_FOUND", $"La entrega {id} no existe."))
        : Results.Ok(new DeliveryResponse(delivery.Id.Value, delivery.CourierId, delivery.Estado.ToString()));
});

app.MapPost("/deliveries/{id:guid}/assign-courier", async (
    Guid id,
    AssignCourierRequest request,
    AssignCourierUseCase useCase,
    ILogger<Program> logger) =>
{
    if (request.CourierId == Guid.Empty)
        return Results.BadRequest(new ErrorResponse("INVALID_COURIER", "El identificador del repartidor es obligatorio."));

    try
    {
        await useCase.ExecuteAsync(new DeliveryId(id), request.CourierId);
        logger.LogInformation(
            "Asignación exitosa. DeliveryId={DeliveryId} CourierId={CourierId}", id, request.CourierId);
        return Results.Ok(new DeliveryResponse(id, request.CourierId, EstadoEntrega.Asignada.ToString()));
    }
    catch (DeliveryNotFoundException ex)
    {
        logger.LogWarning("Entrega no encontrada. DeliveryId={DeliveryId}", id);
        return Results.NotFound(new ErrorResponse("DELIVERY_NOT_FOUND", ex.Message));
    }
    catch (CourierNotAvailableException ex)
    {
        logger.LogWarning(
            "Repartidor no disponible. DeliveryId={DeliveryId} CourierId={CourierId}", id, request.CourierId);
        return Results.Conflict(new ErrorResponse("COURIER_NOT_AVAILABLE", ex.Message));
    }
    catch (DeliveryAlreadyAssignedException ex)
    {
        logger.LogWarning("Entrega ya asignada. DeliveryId={DeliveryId}", id);
        return Results.Conflict(new ErrorResponse("DELIVERY_ALREADY_ASSIGNED", ex.Message));
    }
    catch (Exception ex)
    {
        // Falla controlada: se registra el detalle, pero al cliente solo se le da un mensaje genérico.
        logger.LogError(ex, "Error inesperado al asignar repartidor. DeliveryId={DeliveryId}", id);
        return Results.Problem("Ocurrió un error inesperado.", statusCode: 500);
    }
});

app.Run();

public sealed record AssignCourierRequest(Guid CourierId);
public sealed record DeliveryResponse(Guid DeliveryId, Guid? CourierId, string Estado);
public sealed record ErrorResponse(string Code, string Message);

// Permite que las pruebas de integración arranquen esta aplicación.
public partial class Program { }