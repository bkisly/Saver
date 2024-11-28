using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Repositories;

namespace Saver.FinanceService.Commands;

public record CreateAccountHolderCommand(Guid UserId) : IRequest<CommandResult>;

public class CreateAccountHolderCommandHandler(IAccountHolderRepository accountHolderRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAccountHolderCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateAccountHolderCommand request, CancellationToken cancellationToken)
    {
        var accountHolder = new AccountHolder(request.UserId);
        accountHolderRepository.Add(accountHolder);
        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save changes");
    }
}