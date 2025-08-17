using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; private set; }
        public Guid SaleId { get; private set; }
        public ProductExternalIdentity Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total => (Product.ProductPrice * Quantity) - Discount;

        protected SaleItem() { }

        public SaleItem(ProductExternalIdentity product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}
