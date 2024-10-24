namespace Saver.FinanceService.Domain;

public class RecurringTransactionDefinition(TransactionData data) : Entity<Guid>
{
    public TransactionData TransactionData { get; } = data;
}