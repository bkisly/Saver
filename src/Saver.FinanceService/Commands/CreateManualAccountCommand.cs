using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record CreateManualAccountCommand(string Name, string? Description, string CurrencyCode, decimal InitialBalance) 
    : IRequest<CommandResult>;

public class CreateManualAccountCommandHandler(IAccountHolderService accountHolderService, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateManualAccountCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateManualAccountCommand request, CancellationToken cancellationToken)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        var repository = accountHolderService.Repository;

        if (accountHolder is null)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        var currency = Enumeration.FromName<Currency>(request.CurrencyCode);

        try
        {
            accountHolder.CreateManualAccount(request.Name, currency, request.InitialBalance);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        repository.Update(accountHolder);
        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error(message: "Unable to save changes.");
    }
}