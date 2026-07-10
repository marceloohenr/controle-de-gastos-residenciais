using HouseholdExpenses.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseholdExpenses.Api.Configurations;

/// <summary>Define os limites dos campos e o relacionamento em cascata de pessoas.</summary>
public sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("People");
        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        // O cascade mantém a integridade do histórico sem exclusões manuais de transações no serviço.
        builder.HasMany(x => x.Transactions).WithOne(x => x.Person).HasForeignKey(x => x.PersonId).OnDelete(DeleteBehavior.Cascade);
    }
}

/// <summary>Define precisão monetária, campos obrigatórios e índices de transações.</summary>
public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.Property(x => x.Description).HasMaxLength(180).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.HasIndex(x => x.PersonId);
    }
}
