using FluentValidation;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Commands.Validators;

public class EditManualBankAccountCommandValidator : AbstractValidator<EditManualBankAccountCommand>
{
    public EditManualBankAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.CurrencyCode)
            .Must(Enumeration.HasName<Currency>);
    }
}