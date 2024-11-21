using FluentValidation;
using MediatR;

namespace Saver.FinanceService.Behaviors;

public sealed class ValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators) 
    : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var errors = validationResults
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .ToList();

        if (errors.Count > 0)
            throw new ValidationException(errors);

        var response = await next();
        return response;
    }
}