using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class RecurringTransactionDefinition : EventPublishingEntity<Guid>
{
    public string Cron { get; private set; } = null!;
    public TransactionData TransactionData { get; set; } = null!;

    private RecurringTransactionDefinition()
    { }

    public RecurringTransactionDefinition(TransactionData data, string cron)
    {
        TransactionData = data;
        Cron = cron;
    }

    public void UpdateTransactionData(TransactionData transactionData)
    {
        TransactionData = transactionData;
        AddDomainEvent(new RecurringTransactionUpdatedDomainEvent(this));
    }

    public void UpdateCron(string cron)
    {
        Cron = cron;
        AddDomainEvent(new RecurringTransactionUpdatedDomainEvent(this));
    }
}