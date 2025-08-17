namespace Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities
{
    public class ProductExternalIdentity
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
