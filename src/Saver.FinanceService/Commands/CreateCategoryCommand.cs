﻿using MediatR;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record CreateCategoryCommand(string Name, string? Description) : IRequest<CommandResult>;

public class CreateCategoryCommandHandler(IAccountHolderService accountHolderService) : IRequestHandler<CreateCategoryCommand, CommandResult>
{
    public async Task<CommandResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        var repository = accountHolderService.Repository;

        if (accountHolder is null)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        try
        {
            accountHolder.CreateCategory(request.Name, request.Description);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        repository.Update(accountHolder);
        var result = await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save entities.");
    }
}