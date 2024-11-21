using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Services;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record EditTransactionCommand(Guid TransactionId, Guid AccountId, string Name, string? Description, decimal Value, DateTime CreatedTime, Guid? CategoryId)
    : IRequest<CommandResult>;

public class EditTransactionCommandHandler(
    IAccountHolderService accountHolderService, 
    ITransactionDomainService transactionService, 
    IUnitOfWork unitOfWork)
    : IRequestHandler<EditTransactionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(EditTransactionCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound, "Account holder not found.");
        }

        try
        {
            var category = request.CategoryId.HasValue ? accountHolder.FindCategoryById(request.CategoryId.Value) : null;
            var newData = new TransactionData(request.Name, request.Description, request.Value, category);
            await transactionService.EditTransactionAsync(accountHolder, request.TransactionId, 
                newData, request.CreatedTime);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }
        
        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error(message: "Cannot save changes.");
    }
}