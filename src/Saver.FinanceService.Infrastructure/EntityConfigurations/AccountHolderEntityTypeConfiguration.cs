using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Infrastructure.EntityConfigurations;

internal class AccountHolderEntityTypeConfiguration : IEntityTypeConfiguration<AccountHolder>
{
    public void Configure(EntityTypeBuilder<AccountHolder> builder)
    {
        builder.ToTable("accountHolders");

        builder.Ignore(x => x.DomainEvents);

        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Categories)
            .WithOne();

        builder.HasMany(x => x.Accounts)
            .WithOne()
            .HasForeignKey(x => x.AccountHolderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DefaultAccount)
            .WithMany()
            .HasForeignKey(x => x.DefaultAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.DefaultAccountId)
            .IsUnique();

        builder.HasIndex(x => x.UserId)
            .IsUnique();
    }
}