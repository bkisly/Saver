namespace Saver.FinanceService.Domain;

public class AccountHolder : Entity<Guid>, IAggregateRoot
{
    private readonly List<BankAccount> _accounts = [];
    private readonly List<Category> _categories = [];

    public IReadOnlyList<BankAccount> Account => _accounts.AsReadOnly();
    public IReadOnlyList<Category> Categories => _categories.AsReadOnly();

    public BankAccount DefaultAccount { get; private set; }
}