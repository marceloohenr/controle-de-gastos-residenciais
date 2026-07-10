using HouseholdExpenses.Api.Entities;
using HouseholdExpenses.Api.Persistence;
using HouseholdExpenses.Api.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Tests;

public sealed class SummaryServiceTests
{
    [Fact]
    public async Task Calculates_individual_and_general_totals_with_sqlite()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        await using var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();
        var person = new Person { Name = "Caio", Age = 30 };
        db.People.Add(person);
        db.Transactions.AddRange(
            new Transaction { Description = "Salário", Amount = 1000.50m, Type = TransactionType.Income, Person = person },
            new Transaction { Description = "Aluguel", Amount = 400.25m, Type = TransactionType.Expense, Person = person });
        await db.SaveChangesAsync();

        var result = await new SummaryService(db).GetAsync(default);

        var personSummary = Assert.Single(result.People);
        Assert.Equal(1000.50m, personSummary.TotalIncome);
        Assert.Equal(400.25m, personSummary.TotalExpenses);
        Assert.Equal(600.25m, personSummary.Balance);
        Assert.Equal(600.25m, result.Totals.NetBalance);
    }

    [Fact]
    public async Task Includes_people_without_transactions()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        await using var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();
        db.People.Add(new Person { Name = "Lia", Age = 24 });
        await db.SaveChangesAsync();

        var result = await new SummaryService(db).GetAsync(default);

        var personSummary = Assert.Single(result.People);
        Assert.Equal(0, personSummary.TotalIncome);
        Assert.Equal(0, personSummary.TotalExpenses);
        Assert.Equal(0, personSummary.Balance);
    }
}
