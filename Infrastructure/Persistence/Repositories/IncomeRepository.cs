using Application.Abstractions;
using Application.Queries;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<(IReadOnlyCollection<Income> Items, int TotalItems)> GetHistoryAsync(IncomeHistoryQuery query, CancellationToken cancellationToken)
    {
        var incomesQuery = _dbContext.Incomes.AsNoTracking().AsQueryable();

        if (query.StartDate.HasValue)
        {
            incomesQuery = incomesQuery.Where(income => income.ReceivedOn >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            incomesQuery = incomesQuery.Where(income => income.ReceivedOn <= query.EndDate.Value);
        }

        incomesQuery = incomesQuery
            .OrderByDescending(income => income.ReceivedOn)
            .ThenByDescending(income => income.Id);

        var totalItems = await incomesQuery.CountAsync(cancellationToken);

        if (query.FetchAll != 1)
        {
            incomesQuery = incomesQuery
                .Skip((query.Page - 1) * query.ItemsPage)
                .Take(query.ItemsPage);
        }

        var items = await incomesQuery.ToListAsync(cancellationToken);

        return (items, totalItems);
    }

    public async Task<IReadOnlyCollection<Income>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        return await _dbContext.Incomes
            .AsNoTracking()
            .Where(income => income.ReceivedOn >= startDate && income.ReceivedOn <= endDate)
            .OrderBy(income => income.ReceivedOn)
            .ThenBy(income => income.Id)
            .ToListAsync(cancellationToken);
    }
}