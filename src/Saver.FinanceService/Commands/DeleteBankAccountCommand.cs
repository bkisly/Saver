using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public class DeleteBankAccountCommand(Guid accountId) : IRequest<CommandResult>
{
    public Guid AccountId => accountId;
}

public class DeleteBankAccountCommandHandler(IAccountHolderService accountHolderService, IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteBankAccountCommand, CommandResult>
{
    public async Task<CommandResult> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();

        if (accountHolder is null)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        try
        {
            accountHolder.RemoveAccount(request.AccountId);
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