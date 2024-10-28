using Quartz;
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
        if (!CronExpression.IsValidExpression(cron))
            throw new FinanceDomainException($"Given expression ({cron}) is not a valid cron expression");

        TransactionData = data;
        Cron = cron;
    }

    public void UpdateTransactionData(TransactionData transactionData)
    {
        TransactionData = transactionData;
        // @TODO: raise event that transaction data for recurring was updated
    }

    public void UpdateCron(string cron)
    {
        if (!CronExpression.IsValidExpression(cron))
            throw new FinanceDomainException($"Given expression ({cron}) is not a valid cron expression");

        Cron = cron;
        // @TODO: raise event that cron for recurring was updated
    }
}