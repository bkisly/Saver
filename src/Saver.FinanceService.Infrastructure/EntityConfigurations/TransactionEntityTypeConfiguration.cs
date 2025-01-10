using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Infrastructure.EntityConfigurations;

internal class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasKey(t => t.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Ignore(x => x.DomainEvents);

        var transactionDataBuilder = builder.OwnsOne(x => x.TransactionData);

        transactionDataBuilder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        transactionDataBuilder.Property(x => x.Value)
            .IsRequired();

        transactionDataBuilder.HasOne(x => x.Category)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<BankAccount>()
            .WithMany()
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}