using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
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

        [Fact(DisplayName = "SaleItem should throw exception when product is null")]
        public void Given_NullProduct_When_Created_Then_ShouldThrowArgumentNullException()
        {
            // Arrange
            ProductExternalIdentity product = null;
            var quantity = 1;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => new SaleItem(product, quantity));

            // Assert
            Assert.Contains("Value cannot be null.", ex.Message);
        }

        [Fact(DisplayName = "SaleItem should apply 10% discount for quantity between 4 and 9")]
        public void Given_QuantityBetween4And9_When_Created_Then_ShouldApply10PercentDiscount()
        {
            // Arrange
            var product = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();
            product.ProductPrice = 100.0m;

            var quantity = 5;

            // Act
            var saleItem = new SaleItem(product, quantity);

            // Assert
            Assert.Equal(50.0m, saleItem.Discount); 
            Assert.Equal(450.0m, saleItem.Total); 
        }

        [Fact(DisplayName = "SaleItem should apply 20% discount for quantity between 10 and 20")]
        public void Given_QuantityBetween10And20_When_Created_Then_ShouldApply20PercentDiscount()
        {
            // Arrange
            var product = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();
            product.ProductPrice = 100.0m;

            var quantity = 15;

            // Act
            var saleItem = new SaleItem(product, quantity);

            // Assert
            Assert.Equal(300.0m, saleItem.Discount); 
            Assert.Equal(1200.0m, saleItem.Total);
        }

        [Fact(DisplayName = "SaleItem should not apply discount for quantity less than 4")]
        public void Given_QuantityLessThan4_When_Created_Then_ShouldNotApplyDiscount()
        {
            // Arrange
            var product = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();
            product.ProductPrice = 100.0m;

            var quantity = 3;

            // Act
            var saleItem = new SaleItem(product, quantity);

            // Assert
            Assert.Equal(0.0m, saleItem.Discount);
            Assert.Equal(300.0m, saleItem.Total);
        }
    }
}
