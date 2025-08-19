namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default);
    }
}
