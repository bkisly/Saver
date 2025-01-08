using FluentValidation;

namespace Saver.FinanceService.Commands.Validators;

public class CreateExternalBankAccountCommandValidator : AbstractValidator<CreateExternalBankAccountCommand>
{
    public CreateExternalBankAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.ProviderId)
            .GreaterThan(0);
    }
}