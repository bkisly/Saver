using MediatR;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record CreateRecurringTransactionCommand(string Name, string? Description, decimal Value, Guid CategoryId, Guid AccountId, string Cron)
    : IRequest<CommandResult>;

public class CreateRecurringTransactionCommandHandler(IAccountHolderService accountHolderService)
    : IRequestHandler<CreateRecurringTransactionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateRecurringTransactionCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        try
        {
            var category = accountHolder.FindCategoryById(request.CategoryId);
            var transactionData = new TransactionData(request.Name, request.Description, request.Value, category);
            accountHolder.CreateRecurringTransaction(request.AccountId, transactionData, request.Cron);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        var repository = accountHolderService.Repository;
        repository.Update(accountHolder);
        var result = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save changes");
    }
}