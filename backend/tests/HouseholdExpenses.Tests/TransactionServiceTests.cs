using HouseholdExpenses.Api.DTOs;using HouseholdExpenses.Api.Entities;using HouseholdExpenses.Api.Exceptions;using HouseholdExpenses.Api.Persistence;using HouseholdExpenses.Api.Repositories;using HouseholdExpenses.Api.Services;using Microsoft.EntityFrameworkCore;
namespace HouseholdExpenses.Tests;
public sealed class TransactionServiceTests
{
    private static (TransactionService Service, AppDbContext Db) Create(){var options=new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;var db=new AppDbContext(options);return(new TransactionService(new TransactionRepository(db),new PersonRepository(db)),db);}
    [Fact] public async Task Minor_cannot_register_income(){var (service,db)=Create();db.People.Add(new Person{Name="Ana",Age=16});await db.SaveChangesAsync();var action=()=>service.CreateAsync(new CreateTransactionRequest("Mesada",100,TransactionType.Income,1),default);await Assert.ThrowsAsync<BusinessRuleException>(action);}
    [Fact] public async Task Minor_can_register_expense(){var (service,db)=Create();db.People.Add(new Person{Name="Ana",Age=16});await db.SaveChangesAsync();var result=await service.CreateAsync(new CreateTransactionRequest("Livro",50,TransactionType.Expense,1),default);Assert.Equal(TransactionType.Expense,result.Type);Assert.Equal(50,result.Amount);}
    [Fact] public async Task Unknown_person_is_rejected(){var (service,_)=Create();var action=()=>service.CreateAsync(new CreateTransactionRequest("Conta",50,TransactionType.Expense,99),default);await Assert.ThrowsAsync<NotFoundException>(action);}
}
