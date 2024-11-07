using MediatR;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record CreateTransactionCommand(Guid AccountId, string Name, string? Description, decimal Value, DateTime CreatedTime, Guid? CategoryId)
    : IRequest<CommandResult>;

public class CreateTransactionCommandHandler(IAccountHolderService accountHolderService) 
    : IRequestHandler<CreateTransactionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        try
        {
            var account = accountHolder.FindAccountById(request.AccountId);
            var category = request.CategoryId.HasValue 
                ? accountHolder.FindCategoryById(request.CategoryId.Value) 
                : null;

            var transactionData = new TransactionData(request.Name, request.Description, request.Value, account.Currency, category);
            account.CreateTransaction(transactionData, request.CreatedTime);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        var repository = accountHolderService.Repository;
        repository.Update(accountHolder);
        var result = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error(message: "Unable to save changes.");
    }
}