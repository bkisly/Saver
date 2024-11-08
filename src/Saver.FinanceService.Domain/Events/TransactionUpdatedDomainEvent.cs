using MediatR;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.Events;

public record TransactionUpdatedDomainEvent(
    Guid TransactionId, 
    Guid AccountId, 
    TransactionData OldTransactionData, 
    TransactionData NewTransactionData) : INotification;