using MediatR;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.Events;

public record TransactionCreatedDomainEvent(Guid AccountId, TransactionData Transaction, DateTime CreationDate) 
    : INotification;