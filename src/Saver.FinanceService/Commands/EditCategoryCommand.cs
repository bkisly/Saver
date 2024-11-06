using MediatR;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record EditCategoryCommand(Guid CategoryId, string Name, string? Description) : IRequest<CommandResult>;

public class EditCategoryCommandHandler(IAccountHolderService accountHolderService)
    : IRequestHandler<EditCategoryCommand, CommandResult>
{
    public async Task<CommandResult> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        var repository = accountHolderService.Repository;

        if (accountHolder == null)
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);

        try
        {
            accountHolder.EditCategory(request.CategoryId, request.Name, request.Description);
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