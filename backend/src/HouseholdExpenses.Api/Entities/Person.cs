namespace HouseholdExpenses.Api.Entities;

public sealed class Person
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Age { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Transaction> Transactions { get; set; } = [];
}
