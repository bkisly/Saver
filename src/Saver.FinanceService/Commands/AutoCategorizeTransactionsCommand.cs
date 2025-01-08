using MediatR;
using Microsoft.EntityFrameworkCore;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Infrastructure.ServiceAgents.OpenAI;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public class AutoCategorizeTransactionsCommand(Guid accountId, IEnumerable<Guid>? transactionsToCategorize = null) : IRequest<CommandResult>
{
    public Guid AccountId { get; } = accountId;
    public IEnumerable<Guid>? TransactionsToCategorize { get; } = transactionsToCategorize;
}

public class AutoCategorizeTransactionsCommandHandler(
    IAccountHolderService accountHolderService,
    ITransactionRepository transactionRepository, 
    IOpenAiServiceAgent openAiServiceAgent,
    IUnitOfWork unitOfWork) : IRequestHandler<AutoCategorizeTransactionsCommand, CommandResult>
{
    public async Task<CommandResult> Handle(AutoCategorizeTransactionsCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        if (accountHolder.Accounts.SingleOrDefault(x => x.Id == request.AccountId) is not { } account)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        var transactions = transactionRepository.Transactions
            .Where(x => x.AccountId == request.AccountId && x.TransactionData.Category == null);

        if (request.TransactionsToCategorize is not null)
        {
            transactions = transactions.Where(x => request.TransactionsToCategorize.Contains(x.Id));
        }

        var transactionsList = await transactions.ToListAsync(cancellationToken);
        var categoriesDict = accountHolder.Categories.ToDictionary(x => x.Id, x => x);

        if (transactionsList.Count == 0)
        {
            return CommandResult.Success();
        }

        var categorizedTransactions = await openAiServiceAgent.CategorizeTransactionsAsync(
            transactionsList.Select(x =>
                new TransactionModel
                {
                    TransactionId = x.Id,
                    Name = x.TransactionData.Name,
                    Description = x.TransactionData.Description,
                    Value = x.TransactionData.Value,
                    CurrencyCode = account.Currency.Name,
                    CreatedDate = x.CreationDate,
                    CategoryId = x.TransactionData.Category?.Id
                }), 
            accountHolder.Categories.Select(x => 
                new CategoryModel
                {
                    CategoryId = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }));

        foreach (var (originalTransaction, categorizedTransaction) in transactionsList.Zip(categorizedTransactions))
        {
            if (!categorizedTransaction.CategoryId.HasValue || 
                !categoriesDict.TryGetValue(categorizedTransaction.CategoryId.Value, out var category))
            {
                continue;
            }

            var transactionData = new TransactionData(originalTransaction.TransactionData.Name,
                originalTransaction.TransactionData.Description, originalTransaction.TransactionData.Value, category);
            originalTransaction.EditTransaction(transactionData, originalTransaction.CreationDate);
            transactionRepository.Update(originalTransaction);
        }

        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save entities.");
    }
}