using MediatR;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Domain.Events;

public record RecurringTransactionCreatedDomainEvent(Guid AccountId, RecurringTransactionDefinition RecurringTransaction)
    : INotification;