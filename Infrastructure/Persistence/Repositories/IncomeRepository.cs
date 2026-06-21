using Application.Abstractions;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

// Implementa el puerto del repositorio de ingresos persistiendo agregados mediante EF Core.
public sealed class IncomeRepository : IIncomeRepository
{
    private readonly FinancialManagementDbContext _dbContext;

    public IncomeRepository(FinancialManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Income income, CancellationToken cancellationToken)
    {
        await _dbContext.Incomes.AddAsync(income, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}