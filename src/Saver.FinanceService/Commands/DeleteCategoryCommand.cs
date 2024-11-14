using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record DeleteCategoryCommand(Guid CategoryId) : IRequest<CommandResult>;

public class DeleteCategoryCommandHandler(IAccountHolderService accountHolderService, IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteCategoryCommand, CommandResult>
{
    public async Task<CommandResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();

        if (accountHolder is null)
        {
            return CommandResult.Error(FinanceDomainErrorCode.NotFound);
        }

        try
        {
            accountHolder.RemoveCategory(request.CategoryId);
        }
        catch (FinanceDomainException ex)
        {
            return CommandResult.Error(ex.ErrorCode, ex.Message);
        }

        accountHolderService.Repository.Update(accountHolder);
        var result = await unitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? CommandResult.Success() : CommandResult.Error("Unable to save changes.");
    }
}