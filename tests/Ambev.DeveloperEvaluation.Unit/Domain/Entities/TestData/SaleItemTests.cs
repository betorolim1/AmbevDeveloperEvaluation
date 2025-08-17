using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public class SaleItemTests
    {
        [Fact(DisplayName = "SaleItem should be created with valid product and quantity")]
        public void Given_ValidProductAndQuantity_When_Created_Then_ShouldReturnValidSaleItem()
        {
            // Arrange
            var product = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();
            var quantity = 5;

            // Act
            var saleItem = new SaleItem(product, quantity);

            // Assert
            Assert.NotNull(saleItem);
            Assert.Equal(product, saleItem.Product);
            Assert.Equal(quantity, saleItem.Quantity);
            Assert.Equal(0.0m, saleItem.Discount);
        }

        [Fact(DisplayName = "SaleItem should calculate total correctly")]
        public void Given_SaleItem_When_CalculatingTotal_Then_ShouldReturnCorrectTotal()
        {
            // Arrange
            var product = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();
            product.ProductPrice = 50.0m;
            var saleItem = new SaleItem(product, quantity: 2);

            // Act
            var total = saleItem.Total;

            // Assert
            Assert.Equal(100.0m, total);
        }
    }
}
