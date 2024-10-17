using Microsoft.Extensions.Hosting;

namespace Saver.EventBus.RabbitMQ;

public class RabbitMQEventBus : IEventBus, IHostedService, IDisposable
{
    public Task PublishAsync(IntegrationEvent e)
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}