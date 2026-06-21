using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

// Define la sesión de base de datos de EF Core y mapea las raíces de agregado a tablas de PostgreSQL.
public sealed class FinancialManagementDbContext : DbContext
{
    public FinancialManagementDbContext(DbContextOptions<FinancialManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Income> Incomes => Set<Income>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinancialManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}