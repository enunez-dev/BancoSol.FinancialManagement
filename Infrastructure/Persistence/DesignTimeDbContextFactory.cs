using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

// Crea el DbContext en tiempo de diseño para que las migraciones de EF Core puedan generarse fuera del proceso de la API en ejecución.
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FinancialManagementDbContext>
{
    public FinancialManagementDbContext CreateDbContext(string[] args)
    {
        var webApiPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Web.API"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(webApiPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var rawConnectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("The DefaultConnection string was not found.");

        var optionsBuilder = new DbContextOptionsBuilder<FinancialManagementDbContext>();
        optionsBuilder.UseNpgsql(ConnectionStringParser.Parse(rawConnectionString));

        return new FinancialManagementDbContext(optionsBuilder.Options);
    }
}