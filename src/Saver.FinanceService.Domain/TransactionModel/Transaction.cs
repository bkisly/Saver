namespace Saver.FinanceService.Domain.TransactionModel;

public class Transaction : EventPublishingEntity<Guid>, IAggregateRoot
{
    public override Guid Id { get; protected set; } = Guid.NewGuid();

    private TransactionData _data = null!;
    public TransactionData TransactionData
    {
        get => _data;
        private set => _data = (TransactionData)value.Clone();
    }

    public Guid AccountId { get; }

    public DateTime CreationDate { get; private set; }
    public TransactionType TransactionType => TransactionData.Value > 0 ? TransactionType.Income : TransactionType.Outcome;

    private Transaction()
    { }

    public Transaction(Guid accountId, TransactionData data, DateTime creationDate)
    {
        AccountId = accountId;
        CreationDate = creationDate.ToUniversalTime();
        TransactionData = data;
    }

    public void EditTransaction(TransactionData newTransactionData, DateTime newCreatedDate)
    {
        if (TransactionData != newTransactionData)
        {
            TransactionData = newTransactionData;
        }

        if (newCreatedDate != CreationDate)
        {
            CreationDate = newCreatedDate.ToUniversalTime();
        }
    }

    public void ChangeExchangeRate(decimal exchangeRate)
    {
        if (exchangeRate <= 0)
        {
            throw new FinanceDomainException("Cannot apply 0 or less exchange rate.",
                FinanceDomainErrorCode.InvalidValue);
        }

        TransactionData = new TransactionData(TransactionData.Name, TransactionData.Description,
            TransactionData.Value * exchangeRate, TransactionData.Category);
    }
}