using System.ComponentModel.DataAnnotations;
using HouseholdExpenses.Api.Entities;

namespace HouseholdExpenses.Api.DTOs;

/// <summary>
/// Dados aceitos para inclusão de uma pessoa. A idade anulável permite diferenciar zero de campo ausente.
/// </summary>
public sealed class CreatePersonRequest
{
    [Required, StringLength(120, MinimumLength = 2)]
    public required string Name { get; init; }

    [Required, Range(0, 130)]
    public int? Age { get; init; }
}

/// <summary>Representação pública de uma pessoa cadastrada.</summary>
public sealed record PersonResponse(int Id, string Name, int Age, DateTime CreatedAt);

/// <summary>Dados necessários para registrar uma movimentação financeira.</summary>
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
/// <summary>Representação pública de uma transação, incluindo a pessoa associada.</summary>
public sealed record TransactionResponse(int Id, string Description, decimal Amount, TransactionType Type, int PersonId, string PersonName, DateTime CreatedAt);

/// <summary>Totais financeiros calculados para uma pessoa.</summary>
public sealed record PersonSummaryResponse(int PersonId, string PersonName, decimal TotalIncome, decimal TotalExpenses, decimal Balance);
/// <summary>Totais consolidados de todas as pessoas cadastradas.</summary>
public sealed record GeneralSummaryResponse(decimal TotalIncome, decimal TotalExpenses, decimal NetBalance);
/// <summary>Consolida os totais individuais e gerais em uma única consulta.</summary>
public sealed record SummaryResponse(IReadOnlyList<PersonSummaryResponse> People, GeneralSummaryResponse Totals);
