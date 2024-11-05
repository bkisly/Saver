using MediatR;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record SetAccountAsDefaultCommand(Guid AccountId) : IRequest<CommandResult>;

public class SetAccountAsDefaultCommandHandler(IAccountHolderRepository repository, IAccountHolderService accountHolderService) 
    : IRequestHandler<SetAccountAsDefaultCommand, CommandResult>
{
    public async Task<CommandResult> Handle(SetAccountAsDefaultCommand request, CancellationToken cancellationToken)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        if (accountHolder == null)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        try
        {
            accountHolder.SetDefaultAccount(request.AccountId);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        repository.Update(accountHolder);
        var result = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error(message: "Unable to save changes.");
    }
}