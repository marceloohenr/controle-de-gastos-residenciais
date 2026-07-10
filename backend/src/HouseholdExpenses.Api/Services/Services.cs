using HouseholdExpenses.Api.DTOs;
using HouseholdExpenses.Api.Entities;
using HouseholdExpenses.Api.Exceptions;
using HouseholdExpenses.Api.Interfaces;
using HouseholdExpenses.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Api.Services;

/// <summary>Implementa os casos de uso de pessoas sem expor detalhes do Entity Framework.</summary>
public sealed class PersonService(IPersonRepository repository) : IPersonService
{
    public async Task<IReadOnlyList<PersonResponse>> ListAsync(string? search, CancellationToken ct) =>
        (await repository.ListAsync(search, ct)).Select(Map).ToList();

    public async Task<PersonResponse> CreateAsync(CreatePersonRequest request, CancellationToken ct)
    {
        var person = new Person { Name = request.Name.Trim(), Age = request.Age!.Value };
        await repository.AddAsync(person, ct);
        await repository.SaveAsync(ct);

        return Map(person);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var person = await repository.FindAsync(id, ct) ?? throw new NotFoundException("Pessoa não encontrada.");
        repository.Remove(person);
        await repository.SaveAsync(ct);
    }

    private static PersonResponse Map(Person x) => new(x.Id, x.Name, x.Age, x.CreatedAt);
}

/// <summary>Valida a pessoa e as restrições de idade antes de persistir uma transação.</summary>
public sealed class TransactionService(ITransactionRepository transactions, IPersonRepository people) : ITransactionService
{
    public async Task<IReadOnlyList<TransactionResponse>> ListAsync(int? personId, TransactionType? type, CancellationToken ct) =>
        (await transactions.ListAsync(personId, type, ct)).Select(Map).ToList();

    public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, CancellationToken ct)
    {
        if (!Enum.IsDefined(request.Type))
        {
            throw new BusinessRuleException("Tipo de transação inválido.");
        }

        var person = await people.FindAsync(request.PersonId, ct) ?? throw new NotFoundException("Pessoa não encontrada.");

        // A restrição pertence ao domínio e precisa ser aplicada mesmo para clientes que não sejam a interface web.
        if (person.Age < 18 && request.Type == TransactionType.Income)
        {
            throw new BusinessRuleException("Pessoas menores de 18 anos só podem registrar despesas.");
        }

        var transaction = new Transaction { Description = request.Description.Trim(), Amount = request.Amount, Type = request.Type, PersonId = person.Id, Person = person };
        await transactions.AddAsync(transaction, ct);
        await transactions.SaveAsync(ct);

        return Map(transaction);
    }

    private static TransactionResponse Map(Transaction x) => new(x.Id, x.Description, x.Amount, x.Type, x.PersonId, x.Person.Name, x.CreatedAt);
}

/// <summary>Produz a visão financeira por pessoa e os totais gerais da residência.</summary>
public sealed class SummaryService(AppDbContext db) : ISummaryService
{
    public async Task<SummaryResponse> GetAsync(CancellationToken ct)
    {
        // A consulta parte das pessoas para que moradores sem movimentações também apareçam com totais zerados.
        var registeredPeople = await db.People.AsNoTracking()
            .Include(person => person.Transactions)
            .OrderBy(person => person.Name)
            .ToListAsync(ct);

        var people = registeredPeople.Select(person =>
        {
            var income = person.Transactions
                .Where(transaction => transaction.Type == TransactionType.Income)
                .Sum(transaction => transaction.Amount);
            var expenses = person.Transactions
                .Where(transaction => transaction.Type == TransactionType.Expense)
                .Sum(transaction => transaction.Amount);

            return new PersonSummaryResponse(person.Id, person.Name, income, expenses, income - expenses);
        }).ToList();
        var income = people.Sum(x => x.TotalIncome);
        var expenses = people.Sum(x => x.TotalExpenses);

        return new SummaryResponse(people, new GeneralSummaryResponse(income, expenses, income - expenses));
    }
}
