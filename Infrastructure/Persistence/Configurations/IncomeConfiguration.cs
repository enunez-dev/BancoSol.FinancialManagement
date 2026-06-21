using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

// Configura cómo se persiste el agregado Income en PostgreSQL manteniendo el modelo de dominio libre de atributos de EF.
public sealed class IncomeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.ToTable("incomes");

        builder.HasKey(income => income.Id);

        builder.Property(income => income.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder.Property(income => income.Amount)
            .HasColumnName("amount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(income => income.Description)
            .HasColumnName("description")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(income => income.ReceivedOn)
            .HasColumnName("received_on")
            .IsRequired();

        builder.Property(income => income.Source)
            .HasColumnName("source")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(income => income.Currency)
            .HasColumnName("currency")
            .HasConversion<string>()
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(income => income.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();
    }
}