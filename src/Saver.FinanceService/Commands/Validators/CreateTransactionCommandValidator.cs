using FluentValidation;

namespace Saver.FinanceService.Commands.Validators;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Value)
            .NotEqual(0);
    }
}