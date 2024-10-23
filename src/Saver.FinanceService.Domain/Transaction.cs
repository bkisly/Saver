namespace Saver.FinanceService.Domain;

public class Transaction(TransactionDefinition definition, DateTime creationDate) : Entity<Guid>
{
    private TransactionDefinition _definition = (TransactionDefinition)definition.Clone();
    public TransactionDefinition TransactionDefinition
    {
        get => _definition;
        set => _definition = (TransactionDefinition)value.Clone();
    }

    public DateTime CreationDate { get; } = creationDate;
    public TransactionType TransactionType => TransactionDefinition.Value > 0 ? TransactionType.Income : TransactionType.Outcome;
}