using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; private set; }
        public ProductExternalIdentity Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice => Product.UnitPrice;
        public decimal Discount { get; private set; }
        public decimal Total => (UnitPrice * Quantity) - Discount;

        protected SaleItem() { }

        public SaleItem(ProductExternalIdentity product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            Discount = CalculateDiscount(quantity, UnitPrice);
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
