using MediatR;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Commands;

public record EditBankAccountCommand(string Name, string? Description, string CurrencyCode) : IRequest<CommandResult>;

public class EditBankAccountCommandHandler(IAccountHolderService accountHolderService) : IRequestHandler<EditBankAccountCommand, CommandResult>
{
    public Task<CommandResult> Handle(EditBankAccountCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}