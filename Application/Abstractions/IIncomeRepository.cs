using Domain.Entities;

namespace Application.Abstractions;

// Define el puerto de persistencia que usa la capa de aplicación para almacenar y consultar agregados de ingresos.
public interface IIncomeRepository
{
    Task AddAsync(Income income, CancellationToken cancellationToken);
}