using MediatR;
using Saver.EventBus;
using Saver.EventBus.IntegrationEventLog.Utilities;
using Saver.FinanceService.Infrastructure;

namespace Saver.FinanceService.Behaviors;

public sealed class TransactionBehavior<TRequest, TResult>(
    FinanceDbContext context, 
    IIntegrationEventService<FinanceDbContext> integrationEventService,
    ILogger<TransactionBehavior<TRequest, TResult>> logger) 
    : IPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult>
{
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var response = default(TResult);

        try
        {
            if (context.Database.CurrentTransaction != null)
            {
                return await next();
            }

            var transactionId = await ResilientTransaction.New(context).ExecuteAsync(async () =>
            {
                response = await next();
            });

            await integrationEventService.PublishEventsThroughEventBusAsync(transactionId);

            return response!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception was thrown during processing of the transaction.");
            throw;
        }
    }
}