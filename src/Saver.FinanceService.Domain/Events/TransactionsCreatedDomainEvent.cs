using MediatR;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.Events;

public record TransactionsCreatedDomainEvent(Guid AccountId, IEnumerable<(TransactionData Data, DateTime CreationDate)> Transactions) 
    : INotification;