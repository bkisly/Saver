using MediatR;
using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Infrastructure;

namespace Saver.FinanceService.Behaviors;

public class TransactionBehavior<TRequest, TResult>(FinanceDbContext context) 
    : IPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult>
{
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var response = default(TResult);

        try
        {
            if (context.HasActiveTransaction)
                return await next();

            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await context.BeginTransactionAsync();
                response = await next();
                await context.CommitTransactionAsync(transaction!);
                // @TODO: publish integration events here
            });

            return response!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}