using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Dto;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record CreateManualAccountCommand(CreateBankAccountDto BankAccountDto) : IRequest<CommandResult>;

public class CreateManualAccountCommandHandler(
    IAccountHolderService accountHolderService,
    IAccountHolderRepository repository)
    : IRequestHandler<CreateManualAccountCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateManualAccountCommand request, CancellationToken cancellationToken)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        if (accountHolder is null)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        var dto = request.BankAccountDto;
        var currency = Enumeration.GetAll<Currency>().Single(c => c.Name == dto.CurrencyCode);

        try
        {
            accountHolder.CreateManualAccount(dto.Name, currency, dto.InitialBalance);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        repository.Update(accountHolder);
        var result = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error(message: "Unable to save changes.");
    }
}