using MediatR;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record DeleteRecurringTransactionCommand(Guid RecurringTransactionId) : IRequest<CommandResult>;

public class DeleteRecurringTransactionCommandHandler(IAccountHolderService accountHolderService): IRequestHandler<DeleteRecurringTransactionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(DeleteRecurringTransactionCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        try
        {
            accountHolder.DeleteRecurringTransaction(request.RecurringTransactionId);
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