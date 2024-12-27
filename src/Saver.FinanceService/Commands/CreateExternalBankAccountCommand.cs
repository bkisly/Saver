using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Contracts.BankAccounts;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public class CreateExternalBankAccountCommand(string name, int providerId, string currencyCode) : IRequest<CommandResult<BankAccountDto>>
{
    public string Name => name;
    public int ProviderId => providerId;
    public string CurrencyCode => currencyCode;
}

public class CreateExternalBankAccountCommandHandler(IAccountHolderService accountHolderService, IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateExternalBankAccountCommand, CommandResult<BankAccountDto>>
{
    public async Task<CommandResult<BankAccountDto>> Handle(CreateExternalBankAccountCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
        {
            return CommandResult<BankAccountDto>.Error(FinanceDomainErrorCode.NotFound);
        }

        var repository = accountHolderService.Repository;
        ExternalBankAccount account;

        try
        {
            account = accountHolder.CreateExternalBankAccount(
                request.Name, 
                Enumeration.FromName<Currency>(request.CurrencyCode),
                request.ProviderId);

        }
        catch (FinanceDomainException ex)
        {
            return CommandResult<BankAccountDto>.Error(ex.ErrorCode);
        }

        repository.Update(accountHolder);

        if (!await unitOfWork.SaveEntitiesAsync(cancellationToken))
        {
            return CommandResult<BankAccountDto>.Error("Unable to save entities.");
        }

        var dto = new BankAccountDto
        {
            Balance = account.Balance,
            CurrencyCode = account.Currency.Name,
            Description = null,
            Id = account.Id,
            IsDefault = accountHolder.DefaultAccount?.BankAccount == account,
            IsExternal = true,
            Name = account.Name
        };

        return CommandResult<BankAccountDto>.Success(dto);
    }
}