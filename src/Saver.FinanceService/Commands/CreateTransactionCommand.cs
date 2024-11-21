using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Services;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record CreateTransactionCommand(Guid AccountId, string Name, string? Description, decimal Value, DateTime CreatedTime, Guid? CategoryId)
    : IRequest<CommandResult>;

public class CreateTransactionCommandHandler(
    IAccountHolderService accountHolderService, 
    ITransactionDomainService transactionService, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateTransactionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        try
        {
            accountHolder.FindAccountById(request.AccountId);
            var category = request.CategoryId.HasValue 
                ? accountHolder.FindCategoryById(request.CategoryId.Value) 
                : null;

            var transactionData = new TransactionData(request.Name, request.Description, request.Value, category);
            transactionService.CreateTransaction(accountHolder, request.AccountId, 
                transactionData, request.CreatedTime);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error(message: "Unable to save changes.");
    }
}