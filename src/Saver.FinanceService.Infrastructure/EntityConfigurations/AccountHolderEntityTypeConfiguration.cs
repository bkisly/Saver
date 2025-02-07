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

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasMany(x => x.Categories)
            .WithOne()
            .HasForeignKey(x => x.AccountHolderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Accounts)
            .WithOne()
            .HasForeignKey(x => x.AccountHolderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DefaultAccount)
            .WithOne()
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.UserId)
            .IsUnique();
    }
}