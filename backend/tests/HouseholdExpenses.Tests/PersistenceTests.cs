using HouseholdExpenses.Api.Entities;
using HouseholdExpenses.Api.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Tests;

public sealed class PersistenceTests
{
    [Fact]
    public async Task Deleting_person_deletes_related_transactions()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

        await using var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();
        var person = new Person { Name = "Marina", Age = 28 };
        db.Add(new Transaction
        {
            Description = "Mercado",
            Amount = 150,
            Type = TransactionType.Expense,
            Person = person
        });
        await db.SaveChangesAsync();

        db.People.Remove(person);
        await db.SaveChangesAsync();

        Assert.Empty(await db.Transactions.AsNoTracking().ToListAsync());
    }
}
