using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Infrastructure.Repositories;

public class TransactionRepository(FinanceDbContext context) : ITransactionRepository
{
    public IQueryable<Transaction> Transactions => context.Transactions;

    public async Task<Transaction?> FindByIdAsync(Guid transactionId)
    {
        var transaction = await context.Transactions.SingleOrDefaultAsync(x => x.Id == transactionId);
        return transaction;
    }

    public async Task<IEnumerable<Transaction>> FindByAccountIdAsync(Guid accountId)
    {
        return await Task.FromResult(context.Transactions
            .Where(x => x.AccountId == accountId)
            .ToList());
    }

    public Transaction Add(Transaction transaction)
    {
        return context.Transactions.Add(transaction).Entity;
    }

    public void AddRange(IEnumerable<Transaction> transactions)
    {
        context.Transactions.AddRange(transactions);
    }

    public void Update(Transaction transaction)
    {
        context.Transactions.Update(transaction);
    }

    public void Delete(Guid transactionId)
    {
        var transaction = context.Transactions.SingleOrDefault(x => x.Id == transactionId);
        if (transaction != null)
            context.Transactions.Remove(transaction);
    }

    public void DeleteRange(IEnumerable<Guid> transactionIds)
    {
        var transactionsToRemove = context.Transactions.Where(x => transactionIds.Contains(x.Id));
        if (transactionsToRemove.Any())
            context.Transactions.RemoveRange(transactionsToRemove);
    }
}