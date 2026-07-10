using HouseholdExpenses.Api.Entities;

namespace HouseholdExpenses.Api.Interfaces;

/// <summary>Abstrai a persistência necessária aos casos de uso de pessoas.</summary>
public interface IPersonRepository
{
    Task<IReadOnlyList<Person>> ListAsync(string? search, CancellationToken cancellationToken);
    Task<Person?> FindAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Person person, CancellationToken cancellationToken);
    void Remove(Person person);
    Task SaveAsync(CancellationToken cancellationToken);
}

/// <summary>Abstrai a persistência e os filtros disponíveis para transações.</summary>
public interface ITransactionRepository
{
    Task<IReadOnlyList<Transaction>> ListAsync(int? personId, TransactionType? type, CancellationToken cancellationToken);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
}
