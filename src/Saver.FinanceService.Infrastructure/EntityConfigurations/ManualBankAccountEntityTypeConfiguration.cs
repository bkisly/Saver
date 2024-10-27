using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Infrastructure.EntityConfigurations;

internal class ManualBankAccountEntityTypeConfiguration : IEntityTypeConfiguration<ManualBankAccount>
{
    public void Configure(EntityTypeBuilder<ManualBankAccount> builder)
    {
        builder.HasBaseType<BankAccount>();

        builder.HasMany(x => x.RecurringTransactions)
            .WithOne();
    }
}