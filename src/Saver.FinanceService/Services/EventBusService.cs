using Saver.EventBus;
using Saver.EventBus.IntegrationEventLog.Services;
using Saver.FinanceService.Infrastructure;

namespace Saver.FinanceService.Services;

public class EventBusService(IEventBus eventBus, IIntegrationEventLogService eventLogService, FinanceDbContext context) 
    : IEventBusService
{
    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        throw new NotImplementedException();
    }

    public async Task AddAndSaveIntegrationEventAsync(IntegrationEvent e)
    {
        await eventLogService.SaveEventAsync(e, context.CurrentTransaction ?? 
                                                throw new InvalidOperationException("Cannot save integration event for current transaction being null."));
    }
}