using FluentValidation;
using Quartz;

namespace Saver.FinanceService.Commands.Validators;

public class CreateRecurringTransactionCommandValidator : AbstractValidator<CreateRecurringTransactionCommand>
{
    public CreateRecurringTransactionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Value)
            .NotEqual(0);

        RuleFor(x => x.Cron)
            .Must(CronExpression.IsValidExpression);
    }
}