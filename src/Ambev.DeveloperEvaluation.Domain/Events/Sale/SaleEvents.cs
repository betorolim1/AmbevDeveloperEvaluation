namespace Ambev.DeveloperEvaluation.Domain.Events.Sale
{
    public record SaleCreatedEvent(Guid SaleId);
    public record SaleModifiedEvent(Guid SaleId);
    public record SaleCancelledEvent(Guid SaleId);
}
