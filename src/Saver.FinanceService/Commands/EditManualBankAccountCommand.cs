using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Infrastructure.ServiceAgents.ExchangeRate;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public class EditManualBankAccountCommand(
    Guid accountId,
    string name,
    string currencyCode) : IRequest<CommandResult>
{
    public Guid AccountId => accountId;
    public string Name => name;
    public string CurrencyCode => currencyCode;
}

public class EditManualBankAccountCommandHandler(
    IAccountHolderService accountHolderService, 
    IExchangeRateServiceAgent exchangeRateServiceAgent, 
    IUnitOfWork unitOfWork,
    ILogger<EditCategoryCommandHandler> logger) 
    : IRequestHandler<EditManualBankAccountCommand, CommandResult>
{
    public async Task<CommandResult> Handle(EditManualBankAccountCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        try
        {
            var account = accountHolder.FindAccountById(request.AccountId);
            var newCurrency = Enumeration.FromName<Currency>(request.CurrencyCode);

            var exchangeRate = 1M;
            if (newCurrency != account.Currency)
            {
                try
                {
                    exchangeRate = await exchangeRateServiceAgent.GetExchangeRateAsync(account.Currency, newCurrency);
                }
                catch (ExchangeRateServiceException e)
                {
                    logger.LogError("{message}", e.Message);
                }
            }

            accountHolder.EditManualAccount(request.AccountId, request.Name, newCurrency, exchangeRate);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        accountHolderService.Repository.Update(accountHolder);
        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save changes.");
    }
}