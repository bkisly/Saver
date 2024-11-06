using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class RecurringTransactionDefinition : EventPublishingEntity<Guid>
{
    public string Cron { get; private set; } = null!;
    public TransactionData TransactionData { get; set; } = null!;
    public Guid ManualBankAccountId { get; private set; }

    private RecurringTransactionDefinition()
    { }

    public RecurringTransactionDefinition(TransactionData data, string cron, Guid manualBankAccountId)
    {
        TransactionData = data;
        Cron = cron;
        ManualBankAccountId = manualBankAccountId;
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