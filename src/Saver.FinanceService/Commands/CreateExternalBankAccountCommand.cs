using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public class CreateExternalBankAccountCommand(string name, int providerId, string currencyCode) : IRequest<CommandResult>
{
    public string Name => name;
    public int ProviderId => providerId;
    public string CurrencyCode => currencyCode;
}

public class CreateExternalBankAccountCommandHandler(IAccountHolderService accountHolderService, IUnitOfWork unitOfWork) : IRequestHandler<CreateExternalBankAccountCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateExternalBankAccountCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        var repository = accountHolderService.Repository;

        try
        {
            accountHolder.CreateExternalBankAccount(
                request.Name, 
                Enumeration.FromName<Currency>(request.CurrencyCode),
                request.ProviderId);

        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode);
        }

        repository.Update(accountHolder);
        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save changes.");
    }
}