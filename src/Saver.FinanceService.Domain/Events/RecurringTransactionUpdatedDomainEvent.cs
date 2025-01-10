using MediatR;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Domain.Events;

public record RecurringTransactionUpdatedDomainEvent(RecurringTransactionDefinition NewRecurringTransaction)
    : INotification;