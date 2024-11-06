using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Dto;
using Saver.FinanceService.Infrastructure;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Queries;

public class TransactionQueries(
    IAccountHolderService accountHolderService, 
    IMapper mapper,
    FinanceDbContext context) 
    : ITransactionQueries
{
    public async Task<IEnumerable<TransactionDto>?> GetTransactionsForAccountAsync(Guid accountId)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return null;

        if (accountHolder.Accounts.All(x => x.Id != accountId))
            return null;

        return context.Transactions
            .Where(x => x.AccountId == accountId)
            .ToList()
            .Select(MapFromEntity);
    }

    public async Task<TransactionDto?> GetTransactionByIdAsync(Guid transactionId)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return null;

        var accountIds = accountHolder.Accounts.Select(x => x.Id).ToList();
        var transaction = await context.Transactions
            .Where(x => accountIds.Contains(x.AccountId))
            .SingleOrDefaultAsync(x => x.Id == transactionId);

        return transaction is not null ? MapFromEntity(transaction) : null;
    }

    public async Task<IEnumerable<RecurringTransactionDefinitionDto>?> GetRecurringTransactionDefinitionsForAccountAsync(Guid accountId)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return null;

        if (accountHolder.Accounts.All(x => x.Id != accountId))
            return null;

        return context.RecurringTransactionDefinitions
            .Where(x => x.ManualBankAccountId == accountId)
            .ToList()
            .Select(MapFromEntity);
    }

    public async Task<RecurringTransactionDefinitionDto?> GetRecurringTransactionDefinitionByIdAsync(Guid id)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return null;

        var holderAccountsIds = accountHolder.Accounts.Select(x => x.Id);
        var definition = await context.RecurringTransactionDefinitions
            .Where(x => holderAccountsIds.Contains(x.ManualBankAccountId))
            .SingleOrDefaultAsync(x => x.Id == id);

        return definition is not null ? MapFromEntity(definition) : null;
    }

    private TransactionDto MapFromEntity(Transaction entity)
    {
        var category = entity.TransactionData.Category;
        return new TransactionDto
        {
            Id = entity.Id,
            Name = entity.TransactionData.Name,
            CreatedDate = entity.CreationDate,
            Description = entity.TransactionData.Description,
            Value = entity.TransactionData.Value,
            CurrencyCode = entity.TransactionData.Currency.Name,
            Category = category is not null ? mapper.Map<Category, CategoryDto>(category) : null
        };
    }

    private RecurringTransactionDefinitionDto MapFromEntity(RecurringTransactionDefinition entity)
    {
        var category = entity.TransactionData.Category;
        return new RecurringTransactionDefinitionDto
        {
            Id = entity.Id,
            Name = entity.TransactionData.Name,
            Description = entity.TransactionData.Description,
            Value = entity.TransactionData.Value,
            Cron = entity.Cron,
            Category = category is not null ? mapper.Map<Category, CategoryDto>(category) : null,
            CurrencyCode = entity.TransactionData.Currency.Name
        };
    }
}