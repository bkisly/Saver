namespace Saver.FinanceService.Domain;

public class AccountHolder : Entity<Guid>, IAggregateRoot
{
    private readonly List<Account> _accounts = [];
    private readonly List<Category> _categories = [];

    public IReadOnlyList<Account> Account => _accounts.AsReadOnly();
    public IReadOnlyList<Category> Categories => _categories.AsReadOnly();
}