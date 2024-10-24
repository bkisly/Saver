using CSharpFunctionalExtensions;

namespace Saver.FinanceService.Domain;

public class Transaction(Guid accountId, TransactionData data, DateTime creationDate) : Entity<Guid>, IAggregateRoot
{
    private TransactionData _data = (TransactionData)data.Clone();
    public TransactionData TransactionData
    {
        get => _data;
        set => _data = (TransactionData)value.Clone();
    }

    public Guid AccountId { get; } = accountId;
    public DateTime CreationDate { get; } = creationDate;
    public TransactionType TransactionType => TransactionData.Value > 0 ? TransactionType.Income : TransactionType.Outcome;
}