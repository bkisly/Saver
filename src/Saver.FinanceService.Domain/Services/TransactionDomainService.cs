using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.Services;

public class TransactionDomainService(IAccountHolderRepository accountHolderRepository, ITransactionRepository transactionRepository) 
    : ITransactionDomainService
{
    public Transaction CreateTransaction(AccountHolder accountHolder, Guid accountId, TransactionData transactionData, DateTime creationDate)
    {
        var account = accountHolder.FindAccountById(accountId);

        if (transactionData.Category is not null && !accountHolder.Categories.Contains(transactionData.Category))
        {
            throw new FinanceDomainException(
                "Cannot create a transaction with a category not belonging to any holder's categories.",
                FinanceDomainErrorCode.InvalidOperation);
        }

        var transaction = new Transaction(accountId, transactionData, creationDate);
        account.UpdateBalance(account.Balance + transaction.TransactionData.Value);

        transactionRepository.Add(transaction);
        accountHolderRepository.Update(accountHolder);

        return transaction;
    }

    public IEnumerable<Transaction> CreateTransactions(AccountHolder accountHolder, Guid accountId, IEnumerable<(TransactionData TransactionData, DateTime CreationDate)> transactions)
    {
        var createdTransactions = new List<Transaction>();
        var account = accountHolder.FindAccountById(accountId);

        foreach (var (transactionData, creationDate) in transactions)
        {
            if (transactionData.Category is not null && !accountHolder.Categories.Contains(transactionData.Category))
            {
                throw new FinanceDomainException(
                    "Cannot create a transaction with a category not belonging to any holder's categories.",
                    FinanceDomainErrorCode.InvalidOperation);
            }

            var transaction = new Transaction(accountId, transactionData, creationDate);
            transactionRepository.Add(transaction);
            createdTransactions.Add(transaction);
        }

        account.UpdateBalance(account.Balance + createdTransactions.Sum(x => x.TransactionData.Value));
        accountHolderRepository.Update(accountHolder);

        return createdTransactions;
    }

    public async Task EditTransactionAsync(AccountHolder accountHolder, Guid transactionId, TransactionData newTransactionData, DateTime newCreationDate)
    {
        var belongingAccounts = accountHolder.Accounts.Select(x => x.Id);
        var transaction = await transactionRepository.FindByIdAsync(transactionId) 
                          ?? throw new FinanceDomainException($"Could not find transaction with ID {transactionId}", 
                              FinanceDomainErrorCode.NotFound);

        if (!belongingAccounts.Contains(transaction.AccountId))
        {
            throw new FinanceDomainException(
                $"The transaction with ID {transactionId} does not belong to any of holder's accounts.",
                FinanceDomainErrorCode.InvalidOperation);
        }

        var account = accountHolder.FindAccountById(transactionId);

        if (account is not ManualBankAccount manualAccount)
        {
            throw new FinanceDomainException("Transactions can be edited only for manual accounts.",
                FinanceDomainErrorCode.InvalidOperation);
        }

        var oldTransactionData = transaction.TransactionData;
        transaction.EditTransaction(newTransactionData, newCreationDate);
        transactionRepository.Update(transaction);

        manualAccount.UpdateBalance(manualAccount.Balance + (newTransactionData.Value - oldTransactionData.Value));
        accountHolderRepository.Update(accountHolder);
    }

    public async Task DeleteTransactionAsync(AccountHolder accountHolder, Guid transactionId)
    {
        var belongingAccounts = accountHolder.Accounts.Select(x => x.Id);
        var transaction = await transactionRepository.FindByIdAsync(transactionId) 
                          ?? throw new FinanceDomainException($"Could not find transaction with ID {transactionId}", 
                              FinanceDomainErrorCode.NotFound);

        if (!belongingAccounts.Contains(transaction.AccountId))
        {
            throw new FinanceDomainException(
                $"The transaction with ID {transactionId} does not belong to any of holder's accounts.",
                FinanceDomainErrorCode.InvalidOperation);
        }

        var account = accountHolder.FindAccountById(transaction.AccountId);

        if (account is not ManualBankAccount manualAccount)
        {
            throw new FinanceDomainException("Transactions can be deleted only for manual accounts.",
                FinanceDomainErrorCode.InvalidOperation);
        }

        transactionRepository.Delete(transactionId);

        manualAccount.UpdateBalance(manualAccount.Balance - transaction.TransactionData.Value);
        accountHolderRepository.Update(accountHolder);
    }
}