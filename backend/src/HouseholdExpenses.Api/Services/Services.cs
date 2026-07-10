using HouseholdExpenses.Api.DTOs;
using HouseholdExpenses.Api.Entities;
using HouseholdExpenses.Api.Exceptions;
using HouseholdExpenses.Api.Interfaces;
using HouseholdExpenses.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Api.Services;

public sealed class PersonService(IPersonRepository repository) : IPersonService
{
    public async Task<IReadOnlyList<PersonResponse>> ListAsync(string? search, CancellationToken ct) =>
        (await repository.ListAsync(search, ct)).Select(Map).ToList();
    public async Task<PersonResponse> CreateAsync(CreatePersonRequest request, CancellationToken ct)
    {
        var person = new Person { Name = request.Name.Trim(), Age = request.Age!.Value };
        await repository.AddAsync(person, ct); await repository.SaveAsync(ct); return Map(person);
    }
    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var person = await repository.FindAsync(id, ct) ?? throw new NotFoundException("Pessoa não encontrada.");
        repository.Remove(person); await repository.SaveAsync(ct);
    }
    private static PersonResponse Map(Person x) => new(x.Id, x.Name, x.Age, x.CreatedAt);
}

public sealed class TransactionService(ITransactionRepository transactions, IPersonRepository people) : ITransactionService
{
    public async Task<IReadOnlyList<TransactionResponse>> ListAsync(int? personId, TransactionType? type, CancellationToken ct) =>
        (await transactions.ListAsync(personId, type, ct)).Select(Map).ToList();
    public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, CancellationToken ct)
    {
        if (!Enum.IsDefined(request.Type)) throw new BusinessRuleException("Tipo de transação inválido.");
        var person = await people.FindAsync(request.PersonId, ct) ?? throw new NotFoundException("Pessoa não encontrada.");
        // A restrição pertence ao domínio e precisa ser aplicada mesmo para clientes que não sejam a interface web.
        if (person.Age < 18 && request.Type == TransactionType.Income)
            throw new BusinessRuleException("Pessoas menores de 18 anos só podem registrar despesas.");
        var transaction = new Transaction { Description = request.Description.Trim(), Amount = request.Amount, Type = request.Type, PersonId = person.Id, Person = person };
        await transactions.AddAsync(transaction, ct); await transactions.SaveAsync(ct); return Map(transaction);
    }
    private static TransactionResponse Map(Transaction x) => new(x.Id, x.Description, x.Amount, x.Type, x.PersonId, x.Person.Name, x.CreatedAt);
}

public sealed class SummaryService(AppDbContext db) : ISummaryService
{
    public async Task<SummaryResponse> GetAsync(CancellationToken ct)
    {
        // A consulta parte das pessoas para que moradores sem movimentações também apareçam com totais zerados.
        var people = await db.People.AsNoTracking().OrderBy(x => x.Name).Select(person => new PersonSummaryResponse(
            person.Id, person.Name,
            person.Transactions.Where(x => x.Type == TransactionType.Income).Sum(x => (decimal?)x.Amount) ?? 0,
            person.Transactions.Where(x => x.Type == TransactionType.Expense).Sum(x => (decimal?)x.Amount) ?? 0,
            (person.Transactions.Where(x => x.Type == TransactionType.Income).Sum(x => (decimal?)x.Amount) ?? 0) - (person.Transactions.Where(x => x.Type == TransactionType.Expense).Sum(x => (decimal?)x.Amount) ?? 0))).ToListAsync(ct);
        var income = people.Sum(x => x.TotalIncome); var expenses = people.Sum(x => x.TotalExpenses);
        return new SummaryResponse(people, new GeneralSummaryResponse(income, expenses, income - expenses));
    }
}
