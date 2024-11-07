using MediatR;
using Saver.FinanceService.Domain.AccountHolderModel;
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

        var account = accountHolder.Accounts.SingleOrDefault(x => x.Id == request.AccountId);
        if (account is not ManualBankAccount manualAccount)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound, "Manual account not found.");

        var transaction = await transactionRepository.FindByIdAsync(request.TransactionId);
        if (transaction is null)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound, "Transaction not found");

        var category = accountHolder.Categories.SingleOrDefault(x => x.Id == request.CategoryId);

        try
        {
            var newData = new TransactionData(request.Name, request.Description, request.Value, manualAccount.Currency, category);
            manualAccount.UpdateTransaction(request.TransactionId, transaction.TransactionData, newData);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }


    }
}