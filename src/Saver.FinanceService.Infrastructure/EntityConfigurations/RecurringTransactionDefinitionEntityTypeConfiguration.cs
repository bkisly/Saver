using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Infrastructure.EntityConfigurations;

internal class RecurringTransactionDefinitionEntityTypeConfiguration
    : IEntityTypeConfiguration<RecurringTransactionDefinition>
{
    public void Configure(EntityTypeBuilder<RecurringTransactionDefinition> builder)
    {
        builder.ToTable("recurringTransactionDefinitions");

        builder.Ignore(x => x.DomainEvents);

        builder.HasKey(t => t.Id);

        var transactionDataBuilder = builder.OwnsOne(x => x.TransactionData);

        transactionDataBuilder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        transactionDataBuilder.Property(x => x.Value)
            .IsRequired()
            .HasPrecision(2);

        transactionDataBuilder.HasOne(x => x.Category)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.Cron)
            .IsRequired()
            .HasMaxLength(256);
    }
}