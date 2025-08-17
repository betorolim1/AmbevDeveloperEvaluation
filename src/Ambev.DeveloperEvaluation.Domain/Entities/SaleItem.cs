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
            Discount = CalculateDiscount(quantity, Product.ProductPrice);
        }

        /// <summary>
        /// Return discount based on quantity and price.
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        private decimal CalculateDiscount(int quantity, decimal price)
        {
            throw new NotImplementedException("Discount calculation logic is not implemented yet.");
        }
    }
}
