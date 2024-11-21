using Moq;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Repositories;

namespace Saver.FinanceService.Tests.Domain;

public class TransactionDomainServiceFixture
{
    public Mock<IAccountHolderRepository> AccountHolderRepositoryMock { get; private set; } = null!;
    public Mock<ITransactionRepository> TransactionRepositoryMock { get; private set; } = null!;

    public Guid AccountId => Account.Id;
    public BankAccount Account { get; private set; } = null!;
    public AccountHolder AccountHolder { get; private set; } = null!;

    public TransactionDomainServiceFixture()
    {
        Reset();
    }

    public void Reset()
    {
        AccountHolder = new AccountHolder(Guid.NewGuid());
        Account = AccountHolder.CreateManualAccount("Account", Currency.USD, 30M);
        AccountHolder.CreateCategory("Sample category", null);

        AccountHolderRepositoryMock = new Mock<IAccountHolderRepository>();
        TransactionRepositoryMock = new Mock<ITransactionRepository>();
    }
}