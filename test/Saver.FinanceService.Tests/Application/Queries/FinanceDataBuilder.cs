using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Infrastructure;

namespace Saver.FinanceService.Tests.Application.Queries;

public class FinanceDataBuilder(FinanceDbContext context)
{
    public class AccountHolderBuilder(AccountHolder accountHolder, FinanceDbContext context)
    {
        public AccountHolderBuilder WithManualAccount(string name, Currency? currency = null, decimal initialBalance = 0)
        {
            var selectedCurrency = currency ?? Currency.USD;
            context.Currencies.Attach(selectedCurrency);
            accountHolder.CreateManualAccount(name, selectedCurrency, initialBalance);
            return this;
        }

        public AccountHolderBuilder WithCategory(string name, string? description = null)
        {
            accountHolder.CreateCategory(name, description);
            return this;
        }

        public AccountHolder Build()
        {
            return accountHolder;
        }
    }

    public class TransactionsBuilder
    {
        private readonly List<Transaction> _transactions = [];

        public TransactionsBuilder AddTransaction(string name, decimal value, DateTime date, Guid accountId, Category? category)
        {
            var transactionData = new TransactionData(name, null, value, category);
            var transaction = new Transaction(accountId, transactionData, date);
            _transactions.Add(transaction);
            return this;
        }

        public IEnumerable<Transaction> Build()
        {
            return _transactions;
        }
    }

    public AccountHolderBuilder AddAccountHolder(Guid userId)
    {
        var accountHolder = new AccountHolder(userId);
        context.Add(accountHolder);
        context.SaveChanges();
        return new AccountHolderBuilder(accountHolder, context);
    }

    public TransactionsBuilder AddTransaction(string name, decimal value, DateTime date, Guid accountId, Category? category)
    {
        var builder = new TransactionsBuilder();
        builder.AddTransaction(name, value, date, accountId, category);
        return builder;
    }
}