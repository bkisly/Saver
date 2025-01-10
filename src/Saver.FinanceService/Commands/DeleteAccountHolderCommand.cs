using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Repositories;

namespace Saver.FinanceService.Commands;

public class DeleteAccountHolderCommand(Guid userId) : IRequest<CommandResult>
{
    public Guid UserId => userId;
}

public class DeleteAccountHolderCommandHandler(IAccountHolderRepository repository, IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteAccountHolderCommand, CommandResult>
{
    public async Task<CommandResult> Handle(DeleteAccountHolderCommand request, CancellationToken cancellationToken)
    {
        var accountHolder = await repository.FindByUserIdAsync(request.UserId);

        if (accountHolder is null)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        repository.Delete(accountHolder);
        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save changes.");
    }
}