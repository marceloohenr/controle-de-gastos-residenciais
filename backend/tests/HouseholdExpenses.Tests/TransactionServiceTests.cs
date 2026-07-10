using HouseholdExpenses.Api.DTOs;
using HouseholdExpenses.Api.Entities;
using HouseholdExpenses.Api.Exceptions;
using HouseholdExpenses.Api.Persistence;
using HouseholdExpenses.Api.Repositories;
using HouseholdExpenses.Api.Services;
using Microsoft.EntityFrameworkCore;
namespace HouseholdExpenses.Tests;
public sealed class TransactionServiceTests
{
    private static (TransactionService Service, AppDbContext Db) Create() { var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options; var db = new AppDbContext(options); return (new TransactionService(new TransactionRepository(db), new PersonRepository(db)), db); }
    [Fact] public async Task Minor_cannot_register_income() { var (service, db) = Create(); db.People.Add(new Person { Name = "Ana", Age = 16 }); await db.SaveChangesAsync(); var request = new CreateTransactionRequest { Description = "Mesada", Amount = 100, Type = TransactionType.Income, PersonId = 1 }; var action = () => service.CreateAsync(request, default); await Assert.ThrowsAsync<BusinessRuleException>(action); }
    [Fact] public async Task Minor_can_register_expense() { var (service, db) = Create(); db.People.Add(new Person { Name = "Ana", Age = 16 }); await db.SaveChangesAsync(); var request = new CreateTransactionRequest { Description = "Livro", Amount = 50, Type = TransactionType.Expense, PersonId = 1 }; var result = await service.CreateAsync(request, default); Assert.Equal(TransactionType.Expense, result.Type); Assert.Equal(50, result.Amount); }
    [Fact] public async Task Unknown_person_is_rejected() { var (service, _) = Create(); var request = new CreateTransactionRequest { Description = "Conta", Amount = 50, Type = TransactionType.Expense, PersonId = 99 }; var action = () => service.CreateAsync(request, default); await Assert.ThrowsAsync<NotFoundException>(action); }
}
