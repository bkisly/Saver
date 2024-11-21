using FluentValidation;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Commands.Validators;

public class CreateManualAccountCommandValidator : AbstractValidator<CreateManualAccountCommand>
{
	public CreateManualAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.CurrencyCode)
            .Must(x => Enumeration.GetAll<Currency>().Select(c => c.Name).Contains(x));
    }
}