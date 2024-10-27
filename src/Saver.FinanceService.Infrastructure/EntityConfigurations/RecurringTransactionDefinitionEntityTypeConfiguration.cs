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

        builder.OwnsOne(x => x.TransactionData);

        builder.Property(x => x.Cron)
            .IsRequired();
    }
}