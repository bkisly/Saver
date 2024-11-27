using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Infrastructure.EntityConfigurations;

internal class DefaultBankAccountEntityTypeConfiguration : IEntityTypeConfiguration<DefaultBankAccount>
{
    public void Configure(EntityTypeBuilder<DefaultBankAccount> builder)
    {
        builder.ToTable("defaultBankAccounts");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.AccountHolderId)
            .IsUnique();

        builder.HasOne(x => x.BankAccount)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}