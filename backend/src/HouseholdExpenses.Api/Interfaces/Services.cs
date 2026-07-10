using HouseholdExpenses.Api.DTOs;
using HouseholdExpenses.Api.Entities;

namespace HouseholdExpenses.Api.Interfaces;

public interface IPersonService
{
    Task<IReadOnlyList<PersonResponse>> ListAsync(string? search, CancellationToken ct);
    Task<PersonResponse> CreateAsync(CreatePersonRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}
public interface ITransactionService
{
    Task<IReadOnlyList<TransactionResponse>> ListAsync(int? personId, TransactionType? type, CancellationToken ct);
    Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, CancellationToken ct);
}
public interface ISummaryService { Task<SummaryResponse> GetAsync(CancellationToken ct); }
