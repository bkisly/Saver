using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Infrastructure.ServiceAgents;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record EditManualBankAccountCommand(Guid AccountId, string Name, string? Description, string CurrencyCode) : IRequest<CommandResult>;

public class EditManualBankAccountCommandHandler(IAccountHolderService accountHolderService, IExchangeRateServiceAgent exchangeRateServiceAgent) 
    : IRequestHandler<EditManualBankAccountCommand, CommandResult>
{
    public async Task<CommandResult> Handle(EditManualBankAccountCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        try
        {
            var account = accountHolder.FindAccountById(request.AccountId);
            var newCurrency = Enumeration.FromName<Currency>(request.CurrencyCode);
            var exchangeRate = await exchangeRateServiceAgent.GetExchangeRateAsync(account.Currency, newCurrency);

            accountHolder.EditManualAccount(request.AccountId, request.Name, newCurrency, exchangeRate);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        var repository = accountHolderService.Repository;
        repository.Update(accountHolder);
        var result = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save changes.");
    }
}