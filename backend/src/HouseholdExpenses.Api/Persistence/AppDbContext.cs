using HouseholdExpenses.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenses.Api.Persistence;

/// <summary>Unidade de trabalho do Entity Framework para pessoas e suas transações.</summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Person> People => Set<Person>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
