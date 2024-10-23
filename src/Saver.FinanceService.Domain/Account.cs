namespace Saver.FinanceService.Domain;

public class Account : Entity<Guid>
{
    private readonly List<Transaction> _transactions = [];
    private readonly Guid _accountHolderId;

    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();
}