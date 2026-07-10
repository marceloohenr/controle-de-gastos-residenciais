using System.ComponentModel.DataAnnotations;
using HouseholdExpenses.Api.Entities;

namespace HouseholdExpenses.Api.DTOs;

public sealed class CreatePersonRequest
{
    [Required, StringLength(120, MinimumLength = 2)]
    public required string Name { get; init; }

    [Required, Range(0, 130)]
    public int? Age { get; init; }
}

public sealed record PersonResponse(int Id, string Name, int Age, DateTime CreatedAt);

public sealed class CreateTransactionRequest
{
    [Required, StringLength(180, MinimumLength = 2)]
    public required string Description { get; init; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; init; }

    public TransactionType Type { get; init; }

    [Range(1, int.MaxValue)]
    public int PersonId { get; init; }
}
public sealed record TransactionResponse(int Id, string Description, decimal Amount, TransactionType Type, int PersonId, string PersonName, DateTime CreatedAt);

public sealed record PersonSummaryResponse(int PersonId, string PersonName, decimal TotalIncome, decimal TotalExpenses, decimal Balance);
public sealed record GeneralSummaryResponse(decimal TotalIncome, decimal TotalExpenses, decimal NetBalance);
public sealed record SummaryResponse(IReadOnlyList<PersonSummaryResponse> People, GeneralSummaryResponse Totals);
