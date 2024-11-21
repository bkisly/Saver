using FluentValidation;

namespace Saver.FinanceService.Commands.Validators;

public class EditCategoryCommandValidator : AbstractValidator<EditCategoryCommand>
{
    public EditCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}