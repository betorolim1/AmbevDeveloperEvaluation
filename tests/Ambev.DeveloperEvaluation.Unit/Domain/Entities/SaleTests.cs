using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact(DisplayName = "Sale should return correct total amount after adding items")]
        public void Given_SaleWithItems_When_CalculatingTotal_Then_ShouldReturnCorrectTotalAmount()
        {
            // Arrange
            var product1 = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();
            product1.ProductPrice = 10.0m;

            var product2 = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();
            product2.ProductPrice = 20.0m;

            var sale = SaleTestData.GenerateValidSale();

            // Act
            sale.AddItem(product1, 2);
            sale.AddItem(product2, 1);
            var totalAmount = sale.TotalAmount;

            // Assert
            Assert.Equal(40.0m, totalAmount);
        }

        [Fact(DisplayName = "Sale should validate correctly with valid data")]
        public void Given_ValidSaleData_When_Validated_Then_ShouldReturnValidResult()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act
            var validationResult = sale.Validate();

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Errors);
        }

        [Fact(DisplayName = "Sale should clear items correctly")]
        public void Given_SaleWithItems_When_Cleared_Then_ShouldRemoveAllItems()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            var product = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();

            sale.AddItem(product, 1);

            // Act
            sale.ClearItems();

            // Assert
            Assert.Empty(sale.Items);
        }
    }
}
