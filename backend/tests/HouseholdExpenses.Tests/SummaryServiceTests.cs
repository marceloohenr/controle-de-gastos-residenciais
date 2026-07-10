using HouseholdExpenses.Api.Entities;using HouseholdExpenses.Api.Persistence;using HouseholdExpenses.Api.Services;using Microsoft.EntityFrameworkCore;
namespace HouseholdExpenses.Tests;
public sealed class SummaryServiceTests
{
 [Fact] public async Task Calculates_individual_and_general_totals(){var options=new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;await using var db=new AppDbContext(options);var person=new Person{Name="Caio",Age=30};db.People.Add(person);db.Transactions.AddRange(new Transaction{Description="Salário",Amount=1000,Type=TransactionType.Income,Person=person},new Transaction{Description="Aluguel",Amount=400,Type=TransactionType.Expense,Person=person});await db.SaveChangesAsync();var result=await new SummaryService(db).GetAsync(default);Assert.Equal(600,result.People.Single().Balance);Assert.Equal(600,result.Totals.NetBalance);}
}
