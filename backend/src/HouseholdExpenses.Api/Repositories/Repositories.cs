using HouseholdExpenses.Api.Entities;
using HouseholdExpenses.Api.Interfaces;
using HouseholdExpenses.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Api.Repositories;

/// <summary>Executa as operações de persistência de pessoas com consultas sem rastreamento.</summary>
public sealed class PersonRepository(AppDbContext db) : IPersonRepository
{
    public async Task<IReadOnlyList<Person>> ListAsync(string? search, CancellationToken ct)
    {
        var query = db.People.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(person => person.Name.Contains(search.Trim()));
        }

        return await query.OrderBy(x => x.Name).ToListAsync(ct);
    }

    public Task<Person?> FindAsync(int id, CancellationToken ct) => db.People.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task AddAsync(Person person, CancellationToken ct) => db.People.AddAsync(person, ct).AsTask();

    public void Remove(Person person) => db.People.Remove(person);

    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}

/// <summary>Consulta e persiste transações junto aos dados necessários da pessoa associada.</summary>
public sealed class TransactionRepository(AppDbContext db) : ITransactionRepository
{
    public async Task<IReadOnlyList<Transaction>> ListAsync(int? personId, TransactionType? type, CancellationToken ct)
    {
        var query = db.Transactions.AsNoTracking().Include(x => x.Person).AsQueryable();
        if (personId.HasValue)
        {
            query = query.Where(transaction => transaction.PersonId == personId);
        }

        if (type.HasValue)
        {
            query = query.Where(transaction => transaction.Type == type);
        }

        return await query.OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
    }

    public Task AddAsync(Transaction transaction, CancellationToken ct) => db.Transactions.AddAsync(transaction, ct).AsTask();

    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
