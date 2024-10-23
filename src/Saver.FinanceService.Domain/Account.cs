namespace Saver.FinanceService.Domain;

public class Account : Entity<Guid>
{
    private List<Transaction> _transactions = [];

    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();
}