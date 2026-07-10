using System.ComponentModel.DataAnnotations;
using HouseholdExpenses.Api.Entities;

namespace HouseholdExpenses.Api.DTOs;

public sealed record CreatePersonRequest(
    [property: Required, StringLength(120, MinimumLength = 2)] string Name,
    [property: Required, Range(0, 130)] int? Age);
public sealed record PersonResponse(int Id, string Name, int Age, DateTime CreatedAt);

public sealed record CreateTransactionRequest(
    [property: Required, StringLength(180, MinimumLength = 2)] string Description,
    [property: Range(typeof(decimal), "0.01", "79228162514264337593543950335")] decimal Amount,
    TransactionType Type,
    [property: Range(1, int.MaxValue)] int PersonId);
public sealed record TransactionResponse(int Id, string Description, decimal Amount, TransactionType Type, int PersonId, string PersonName, DateTime CreatedAt);

public sealed record PersonSummaryResponse(int PersonId, string PersonName, decimal TotalIncome, decimal TotalExpenses, decimal Balance);
public sealed record GeneralSummaryResponse(decimal TotalIncome, decimal TotalExpenses, decimal NetBalance);
public sealed record SummaryResponse(IReadOnlyList<PersonSummaryResponse> People, GeneralSummaryResponse Totals);
