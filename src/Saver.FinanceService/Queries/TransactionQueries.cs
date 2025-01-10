using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Contracts.Categories;
using Saver.FinanceService.Contracts.Transactions;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;
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

        var account = accountHolder.Accounts.SingleOrDefault(x => x.Id == accountId);
        if (account is null)
            return null;

        return context.Transactions
            .Where(x => x.AccountId == accountId)
            .ToList()
            .Select(x => MapFromEntity(x, account));
    }

    public async Task<TransactionDto?> GetTransactionByIdAsync(Guid transactionId)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return null;

        var accountIds = accountHolder.Accounts.Select(x => x.Id).ToList();
        var transaction = await context.Transactions
            .Where(x => accountIds.Contains(x.AccountId))
            .SingleOrDefaultAsync(x => x.Id == transactionId);

        if (transaction is null)
            return null;

        var account = accountHolder.Accounts.Single(x => x.Id == transaction.AccountId);
        return MapFromEntity(transaction, account);
    }

    public async Task<IEnumerable<RecurringTransactionDefinitionDto>?> GetRecurringTransactionDefinitionsForAccountAsync(Guid accountId)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return null;

        var account = accountHolder.Accounts.SingleOrDefault(x => x.Id == accountId);
        if (account is null)
            return null;

        return context.RecurringTransactionDefinitions
            .Where(x => x.ManualBankAccountId == accountId)
            .ToList()
            .Select(x => MapFromEntity(x, account));
    }

    public async Task<RecurringTransactionDefinitionDto?> GetRecurringTransactionDefinitionByIdAsync(Guid id)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return null;

        var holderAccountsIds = accountHolder.Accounts.Select(x => x.Id);
        var definition = await context.RecurringTransactionDefinitions
            .Where(x => holderAccountsIds.Contains(x.ManualBankAccountId))
            .SingleOrDefaultAsync(x => x.Id == id);

        if (definition is null)
            return null;

        var account = accountHolder.Accounts.Single(x => x.Id == definition.ManualBankAccountId);
        return MapFromEntity(definition, account);
    }

    private TransactionDto MapFromEntity(Transaction entity, BankAccount account)
    {
        var category = entity.TransactionData.Category;
        return new TransactionDto
        {
            Id = entity.Id,
            Name = entity.TransactionData.Name,
            CreatedDate = entity.CreationDate,
            Description = entity.TransactionData.Description,
            Value = entity.TransactionData.Value,
            CurrencyCode = account.Currency.Name,
            Category = category is not null ? mapper.Map<Category, CategoryDto>(category) : null
        };
    }

    private RecurringTransactionDefinitionDto MapFromEntity(RecurringTransactionDefinition entity, BankAccount account)
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
            CurrencyCode = account.Currency.Name
        };
    }
}