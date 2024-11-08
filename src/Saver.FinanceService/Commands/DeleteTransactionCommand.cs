using MediatR;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record DeleteTransactionCommand(Guid TransactionId) : IRequest<CommandResult>;

public class DeleteTransactionCommandHandler(IAccountHolderService accountHolderService, ITransactionRepository transactionRepository)
    : IRequestHandler<DeleteTransactionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        try
        {
            if (await transactionRepository.FindByIdAsync(request.TransactionId) is not { } transaction)
                return CommandResult.Error(FinanceDomainErrorCode.NotFound);

            accountHolder.DeleteTransaction(transaction.AccountId, transaction.Id, transaction.TransactionData);
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