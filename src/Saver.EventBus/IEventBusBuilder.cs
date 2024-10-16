using Microsoft.Extensions.DependencyInjection;

namespace Saver.EventBus;

public interface IEventBusBuilder
{
    public IServiceCollection Services { get; }
}