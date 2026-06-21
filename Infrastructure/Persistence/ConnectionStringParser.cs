using Npgsql;

namespace Infrastructure.Persistence;

// Convierte una URI de PostgreSQL en una cadena de conexión estándar de Npgsql preservando las cadenas que ya son válidas.
public static class ConnectionStringParser
{
    public static string Parse(string rawConnectionString)
    {
        if (string.IsNullOrWhiteSpace(rawConnectionString))
        {
            throw new InvalidOperationException("A PostgreSQL connection string is required.");
        }

        if (!rawConnectionString.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase) &&
            !rawConnectionString.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
        {
            return rawConnectionString;
        }

        var uri = new Uri(rawConnectionString);
        var userInfo = uri.UserInfo.Split(':', 2, StringSplitOptions.TrimEntries);

        if (userInfo.Length != 2)
        {
            throw new InvalidOperationException("The PostgreSQL URI must include both username and password.");
        }

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port,
            Database = uri.AbsolutePath.Trim('/'),
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = Uri.UnescapeDataString(userInfo[1]),
            SslMode = SslMode.Require
        };

        return builder.ConnectionString;
    }
}