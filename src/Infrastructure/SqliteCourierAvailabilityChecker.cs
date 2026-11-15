using Logistica.Application;
using Microsoft.Data.Sqlite;

namespace Logistica.Infrastructure;

public sealed class SqliteCourierAvailabilityChecker : ICourierAvailabilityChecker
{
    private readonly string _connectionString;

    public SqliteCourierAvailabilityChecker(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Un repartidor está disponible si existe y su ruta no está saturada.
    public async Task<bool> IsAvailableAsync(Guid courierId)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT route_saturated FROM couriers WHERE id = $id";
        command.Parameters.AddWithValue("$id", courierId.ToString());

        var result = await command.ExecuteScalarAsync();
        if (result is null)
            return false;

        return Convert.ToInt32(result) == 0;
    }
}