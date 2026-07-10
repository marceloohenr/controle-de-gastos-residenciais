namespace HouseholdExpenses.Api.Entities;

/// <summary>
/// Registra uma entrada ou saída financeira vinculada obrigatoriamente a uma pessoa.
/// </summary>
public sealed class Transaction
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public int PersonId { get; set; }
    public required Person Person { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Define se uma movimentação aumenta ou reduz o saldo da pessoa.
/// </summary>
public enum TransactionType { Income = 1, Expense = 2 }
