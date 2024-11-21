using MediatR;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Domain.Events;

public record AccountCurrencyChangedDomainEvent(Guid AccountId, Currency NewCurrency, decimal ExchangeRate)
    : INotification;