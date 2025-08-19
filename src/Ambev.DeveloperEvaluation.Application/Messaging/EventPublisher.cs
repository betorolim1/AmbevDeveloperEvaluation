using Ambev.DeveloperEvaluation.Domain.Services;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Messaging
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IBus _bus;

        public EventPublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        {
            await _bus.Publish(@event);
            Console.WriteLine($"[Event Published] {typeof(T).Name} - {@event}");
        }
    }
}
