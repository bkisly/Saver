using FluentValidation;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Commands.Validators;

public class CreateManualAccountCommandValidator : AbstractValidator<CreateManualAccountCommand>
{
	public CreateManualAccountCommandValidator()
    {
        RuleFor(x => x.BankAccountDto.Name)
            .NotEmpty();

        RuleFor(x => x.BankAccountDto.CurrencyCode)
            .Must(x => Enumeration.GetAll<Currency>().Select(c => c.Name).Contains(x));
    }
}