using MediatR;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.Events;

public record TransactionUpdatedDomainEvent(Guid TransactionId, TransactionData NewTransactionData) : INotification;