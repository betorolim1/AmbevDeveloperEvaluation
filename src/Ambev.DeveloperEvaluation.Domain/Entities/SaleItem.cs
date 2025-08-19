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
            Product = product ?? throw new ArgumentNullException(nameof(product));

            Quantity = quantity;

            Discount = ApplyDiscount();
        }

        private decimal ApplyDiscount()
        {
            if (Quantity >= 4 && Quantity < 10)
               return Product.ProductPrice * Quantity * 0.1m; // 10%
            else if (Quantity >= 10 && Quantity <= 20)
                return Product.ProductPrice * Quantity * 0.2m; // 20%
            else
                return 0;
        }
    }
}
