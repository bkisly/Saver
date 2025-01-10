using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Infrastructure.EntityConfigurations;

internal class ExternalBankAccountEntityTypeConfiguration : IEntityTypeConfiguration<ExternalBankAccount>
{
    public void Configure(EntityTypeBuilder<ExternalBankAccount> builder)
    {
        builder.HasBaseType<BankAccount>();
    }
}