using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public class CreateRecurringTransactionCommand(
    string name,
    string? description,
    decimal value,
    Guid? categoryId,
    Guid accountId,
    string cron) : IRequest<CommandResult>
{
    public string Name => name;
    public string? Description => description;
    public decimal Value => value;
    public Guid? CategoryId => categoryId;
    public Guid AccountId => accountId;
    public string Cron => cron;
}

public class CreateRecurringTransactionCommandHandler(IAccountHolderService accountHolderService, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateRecurringTransactionCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateRecurringTransactionCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        try
        {
            Category? category = null;
            if (request.CategoryId.HasValue)
            {
                category = accountHolder.FindCategoryById(request.CategoryId.Value);
            }

            var transactionData = new TransactionData(request.Name, request.Description, request.Value, category);
            accountHolder.CreateRecurringTransaction(request.AccountId, transactionData, request.Cron);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        var repository = accountHolderService.Repository;
        repository.Update(accountHolder);
        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save changes");
    }
}