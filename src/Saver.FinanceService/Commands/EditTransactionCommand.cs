using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Services;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public class EditTransactionCommand(
    Guid transactionId,
    Guid accountId,
    string name,
    decimal value,
    DateTime createdDate,
    string? description,
    Guid? categoryId)
    : IRequest<CommandResult>
{
    public Guid TransactionId => transactionId;
    public Guid AccountId => accountId;
    public string Name => name;
    public string? Description => description;
    public decimal Value => value;
    public DateTime CreatedDate => createdDate;
    public Guid? CategoryId => categoryId;
}

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
                newData, request.CreatedDate.ToUniversalTime());
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }
        
        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error(message: "Cannot save changes.");
    }
}