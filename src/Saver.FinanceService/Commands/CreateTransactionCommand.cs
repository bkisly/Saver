using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Services;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public class CreateTransactionCommand(
    Guid accountId,
    string name,
    string? description,
    decimal value,
    DateTime createdDate,
    Guid? categoryId) : IRequest<CommandResult>
{
    public Guid AccountId => accountId;
    public string Name => name;
    public string? Description => description;
    public decimal Value => value;
    public Guid? CategoryId => categoryId;
    public DateTime CreatedDate => createdDate;
}

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
                transactionData, request.CreatedDate);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error(message: "Unable to save changes.");
    }
}