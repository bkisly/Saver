using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Services;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record DeleteTransactionCommand(Guid TransactionId) : IRequest<CommandResult>;

public class DeleteTransactionCommandHandler(
    IAccountHolderService accountHolderService, 
    ITransactionDomainService transactionService, 
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTransactionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        try
        {
            await transactionService.DeleteTransactionAsync(accountHolder, request.TransactionId);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save changes.");
    }
}