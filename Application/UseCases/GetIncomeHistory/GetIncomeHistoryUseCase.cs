using Application.Abstractions;
using Application.DTOs;
using Application.Queries;
using Application.Services;

namespace Application.UseCases.GetIncomeHistory;

// Orquesta el caso de uso de consulta del historial de ingresos aplicando filtros de fecha y reglas de paginaciÃ³n.
public sealed class GetIncomeHistoryUseCase
{
    private readonly IIncomeRepository _incomeRepository;

    public GetIncomeHistoryUseCase(IIncomeRepository incomeRepository)
    {
        _incomeRepository = incomeRepository;
    }

    public async Task<PaginatedIncomeHistoryResponse> ExecuteAsync(IncomeHistoryQuery query, CancellationToken cancellationToken)
    {
        if (query.Page <= 0)
        {
            throw new ArgumentException("La pÃ¡gina debe ser mayor que cero.");
        }

        if (query.ItemsPage <= 0)
        {
            throw new ArgumentException("La cantidad de elementos por pÃ¡gina debe ser mayor que cero.");
        }

        if (query.StartDate.HasValue && query.EndDate.HasValue && query.StartDate > query.EndDate)
        {
            throw new ArgumentException("La fecha inicial no puede ser mayor que la fecha final.");
        }

        var (items, totalItems) = await _incomeRepository.GetHistoryAsync(query, cancellationToken);
        var effectiveItemsPage = query.FetchAll == 1 ? totalItems : query.ItemsPage;
        var totalPages = query.FetchAll == 1
            ? (totalItems > 0 ? 1 : 0)
            : (int)Math.Ceiling(totalItems / (double)query.ItemsPage);

        return new PaginatedIncomeHistoryResponse
        {
            Items = items.Select(IncomeMapper.ToResponse).ToArray(),
            Page = query.FetchAll == 1 ? 1 : query.Page,
            ItemsPage = query.FetchAll == 1 ? effectiveItemsPage : query.ItemsPage,
            TotalItems = totalItems,
            TotalPages = totalPages,
            FetchAll = query.FetchAll == 1,
            StartDate = query.StartDate,
            EndDate = query.EndDate
        };
    }
}