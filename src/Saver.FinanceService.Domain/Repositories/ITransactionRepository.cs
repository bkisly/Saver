using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    IQueryable<Transaction> Transactions { get; }
    Task<Transaction?> FindByIdAsync(Guid transactionId);
    Task<IEnumerable<Transaction>> FindByAccountIdAsync(Guid accountId);
    Transaction Add(Transaction transaction);
    void AddRange(IEnumerable<Transaction> transactions);
    void Update(Transaction transaction);
    void Delete(Guid transactionId);
    void DeleteRange(IEnumerable<Guid> transactionIds);
}