using MediatR;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record EditTransactionCommand(Guid TransactionId, Guid AccountId, string Name, string? Description, decimal Value, DateTime CreatedTime, Guid? CategoryId)
    : IRequest<CommandResult>;

public class EditTransactionCommandHandler(IAccountHolderService accountHolderService, ITransactionRepository transactionRepository)
    : IRequestHandler<EditTransactionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(EditTransactionCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound, "Account holder not found.");

        var transaction = await transactionRepository.FindByIdAsync(request.TransactionId);
        if (transaction is null)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound, "Transaction not found");

        try
        {
            var category = request.CategoryId.HasValue ? accountHolder.FindCategoryById(request.CategoryId.Value) : null;
            var newData = new TransactionData(request.Name, request.Description, request.Value, category);
            transaction.EditTransaction(newData, request.CreatedTime);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }
        
        transactionRepository.Update(transaction);
        var result = await transactionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error(message: "Cannot save changes.");
    }
}