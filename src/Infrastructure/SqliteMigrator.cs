using System.Reflection;
using Microsoft.Data.Sqlite;

namespace Logistica.Infrastructure;

public static class SqliteMigrator
{
    // Ejecuta, en orden, los scripts .sql que viajan dentro del programa.
    // Como los scripts usan CREATE TABLE IF NOT EXISTS, se pueden ejecutar varias veces sin romper nada.
    public static void Migrate(string connectionString)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var scripts = assembly.GetManifestResourceNames()
            .Where(name => name.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToList();

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        foreach (var script in scripts)
        {
            using var stream = assembly.GetManifestResourceStream(script)!;
            using var reader = new StreamReader(stream);
            var sql = reader.ReadToEnd();

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }
    }
}