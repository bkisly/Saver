using CSharpFunctionalExtensions;

namespace Saver.FinanceService.Domain.TransactionModel;

public class Transaction : Entity<Guid>, IAggregateRoot
{
    private TransactionData _data = null!;
    public TransactionData TransactionData
    {
        get => _data;
        set => _data = (TransactionData)value.Clone();
    }

    public Guid AccountId { get; }
    public DateTime CreationDate { get; }
    public TransactionType TransactionType => TransactionData.Value > 0 ? TransactionType.Income : TransactionType.Outcome;

    private Transaction()
    { }

    public Transaction(Guid accountId, TransactionData data, DateTime creationDate)
    {
        AccountId = accountId;
        CreationDate = creationDate;
        TransactionData = (TransactionData)data.Clone();
    }
}