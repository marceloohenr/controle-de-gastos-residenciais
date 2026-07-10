using HouseholdExpenses.Api.Entities;

namespace HouseholdExpenses.Api.Interfaces;

public interface IPersonRepository
{
    Task<IReadOnlyList<Person>> ListAsync(string? search, CancellationToken cancellationToken);
    Task<Person?> FindAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Person person, CancellationToken cancellationToken);
    void Remove(Person person);
    Task SaveAsync(CancellationToken cancellationToken);
}

public interface ITransactionRepository
{
    Task<IReadOnlyList<Transaction>> ListAsync(int? personId, TransactionType? type, CancellationToken cancellationToken);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
}
