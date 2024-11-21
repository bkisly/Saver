using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Infrastructure.EntityConfigurations;

internal class CurrencyEntityTypeConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("currencies");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(3);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasData(Enumeration.GetAll<Currency>());
    }
}