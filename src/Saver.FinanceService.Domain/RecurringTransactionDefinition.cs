using Quartz;

namespace Saver.FinanceService.Domain;

public class RecurringTransactionDefinition : EventPublishingEntity<Guid>
{
    private string _cron = null!;
    public string Cron
    {
        get => _cron;
        set
        {
            if (!CronExpression.IsValidExpression(value))
                throw new FinanceDomainException($"Given expression ({value}) is not a valid cron expression");
            
            _cron = value;
        }
    }

    public TransactionData TransactionData { get; set; }

    public RecurringTransactionDefinition(TransactionData data, string cron)
    {
        TransactionData = data;
        Cron = cron;
    }
}