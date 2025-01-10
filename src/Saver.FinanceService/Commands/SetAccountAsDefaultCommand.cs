using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public class SetAccountAsDefaultCommand(Guid accountId) : IRequest<CommandResult>
{
    public Guid AccountId => accountId;
}

public class SetAccountAsDefaultCommandHandler(IAccountHolderService accountHolderService, IUnitOfWork unitOfWork) 
    : IRequestHandler<SetAccountAsDefaultCommand, CommandResult>
{
    public async Task<CommandResult> Handle(SetAccountAsDefaultCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        try
        {
            accountHolder.SetDefaultAccount(request.AccountId);
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