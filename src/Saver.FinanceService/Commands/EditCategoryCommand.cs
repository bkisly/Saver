using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public class EditCategoryCommand(Guid categoryId, string name, string? description) : IRequest<CommandResult>
{
    public Guid CategoryId => categoryId;
    public string Name => name;
    public string? Description => description;
}

public class EditCategoryCommandHandler(IAccountHolderService accountHolderService, IUnitOfWork unitOfWork)
    : IRequestHandler<EditCategoryCommand, CommandResult>
{
    public async Task<CommandResult> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        try
        {
            accountHolder.EditCategory(request.CategoryId, request.Name, request.Description);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        accountHolderService.Repository.Update(accountHolder);
        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error(message: "Unable to save changes.");
    }
}