﻿using MediatR;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record DeleteBankAccountCommand(Guid AccountId) : IRequest<CommandResult>;

public class DeleteBankAccountCommandHandler(IAccountHolderService accountHolderService) 
    : IRequestHandler<DeleteBankAccountCommand, CommandResult>
{
    public async Task<CommandResult> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        var repository = accountHolderService.Repository;

        if (accountHolder is null)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        try
        {
            accountHolder.RemoveAccount(request.AccountId);
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