namespace HouseholdExpenses.Api.Entities;

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

public enum TransactionType { Income = 1, Expense = 2 }
