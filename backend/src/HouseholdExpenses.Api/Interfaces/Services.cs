using HouseholdExpenses.Api.DTOs;
using HouseholdExpenses.Api.Entities;

namespace HouseholdExpenses.Api.Interfaces;

/// <summary>Coordena os casos de uso de cadastro, consulta e exclusão de pessoas.</summary>
public interface IPersonService
{
    Task<IReadOnlyList<PersonResponse>> ListAsync(string? search, CancellationToken ct);
    Task<PersonResponse> CreateAsync(CreatePersonRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}
/// <summary>Coordena transações e aplica as regras financeiras relacionadas à idade da pessoa.</summary>
public interface ITransactionService
{
    Task<IReadOnlyList<TransactionResponse>> ListAsync(int? personId, TransactionType? type, CancellationToken ct);
    Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, CancellationToken ct);
}
/// <summary>Calcula os saldos individuais e o consolidado da residência.</summary>
public interface ISummaryService { Task<SummaryResponse> GetAsync(CancellationToken ct); }
