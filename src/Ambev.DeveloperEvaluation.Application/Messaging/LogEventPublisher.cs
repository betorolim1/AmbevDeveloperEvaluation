using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Common.Messaging
{
    public class LogEventPublisher : IEventPublisher
    {
        public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"[Event Published] {typeof(T).Name} - {@event}");
            return Task.CompletedTask;
        }
    }
}
