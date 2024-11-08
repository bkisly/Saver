using Saver.FinanceService.Domain.Events;

namespace Saver.FinanceService.Domain.TransactionModel;

public class Transaction : EventPublishingEntity<Guid>, IAggregateRoot
{
    private TransactionData _data = null!;
    public TransactionData TransactionData
    {
        get => _data;
        set => _data = (TransactionData)value.Clone();
    }

    public Guid AccountId { get; }

    public DateTime CreationDate { get; private set; }
    public TransactionType TransactionType => TransactionData.Value > 0 ? TransactionType.Income : TransactionType.Outcome;

    private Transaction()
    { }

    public Transaction(Guid accountId, TransactionData data, DateTime creationDate)
    {
        AccountId = accountId;
        CreationDate = creationDate;
        TransactionData = (TransactionData)data.Clone();
    }

    public void EditTransaction(TransactionData newTransactionData, DateTime newCreatedDate)
    {
        if (TransactionData != newTransactionData)
        {
            var oldData = TransactionData;
            TransactionData = newTransactionData;
            AddDomainEvent(new TransactionUpdatedDomainEvent(Id, AccountId, oldData, TransactionData));
        }

        if (newCreatedDate != CreationDate)
            CreationDate = newCreatedDate;
    }
}