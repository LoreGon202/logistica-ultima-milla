using Logistica.Application;
using Logistica.Domain;
using Microsoft.Data.Sqlite;

namespace Logistica.Infrastructure;

public sealed class SqliteDeliveryRepository : IDeliveryRepository
{
    private readonly string _connectionString;

    public SqliteDeliveryRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Delivery?> GetAsync(DeliveryId id)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText =
            "SELECT address, courier_id, estado FROM deliveries WHERE id = $id";
        command.Parameters.AddWithValue("$id", id.Value.ToString());

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var address = new DeliveryAddress(reader.GetString(0));
        Guid? courierId = reader.IsDBNull(1) ? null : Guid.Parse(reader.GetString(1));
        var estado = Enum.Parse<EstadoEntrega>(reader.GetString(2));

        return Delivery.Restore(id, address, courierId, estado);
    }

    public async Task SaveAsync(Delivery delivery)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO deliveries (id, address, courier_id, estado)
            VALUES ($id, $address, $courierId, $estado)
            ON CONFLICT(id) DO UPDATE SET
                address    = excluded.address,
                courier_id = excluded.courier_id,
                estado     = excluded.estado";

        command.Parameters.AddWithValue("$id", delivery.Id.Value.ToString());
        command.Parameters.AddWithValue("$address", delivery.Address.Value);
        command.Parameters.AddWithValue("$courierId",
            delivery.CourierId.HasValue ? delivery.CourierId.Value.ToString() : DBNull.Value);
        command.Parameters.AddWithValue("$estado", delivery.Estado.ToString());

        await command.ExecuteNonQueryAsync();
    }
}