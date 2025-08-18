using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact(DisplayName = "Sale should be created with valid data")]
        public void Given_ValidSaleData_When_Created_Then_ShouldReturnValidSale()
        {
            var dateNow = DateTime.Now;

            var sale = new Sale(saleNumber: "1234567890123",
                                date: dateNow,
                                customer: CustomerExternalIdentityTestData.GenerateValidCustomerExternalIdentity(),
                                branch: BranchExternalIdentityTestData.GenerateValidBranchExternalIdentity(),
                                cancelled: false);

            Assert.NotNull(sale);
            Assert.NotEqual(Guid.Empty, sale.Id);
            Assert.NotEmpty(sale.SaleNumber);
            Assert.Equal(dateNow.Date, sale.Date.Date);
            Assert.NotNull(sale.Customer);
            Assert.NotNull(sale.Branch);
            Assert.False(sale.Cancelled);
        }

        [Fact(DisplayName = "Sale should be created with update constructor")]
        public void Given_ValidSaleData_When_CreatedWithUpdateConstructor_Then_ShouldReturnValidSale()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act
            var updatedSale = new Sale(sale.Id, sale.Cancelled);

            // Assert
            Assert.NotNull(updatedSale);
            Assert.Equal(sale.Id, updatedSale.Id);
            Assert.Equal(sale.Cancelled, updatedSale.Cancelled);
        }

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

        [Theory(DisplayName = "Sale should be cancelled correctly")]
        [InlineData(true)]
        [InlineData(false)]
        public void Given_Sale_When_Cancelled_Then_ShouldSetCancelledFlag(bool cancelled)
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act
            sale.SetCancelled(cancelled);

            // Assert
            Assert.Equal(cancelled, sale.Cancelled);
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

        [Fact(DisplayName = "Sale should validate correctly with invalid data")]
        public void Given_InvalidSaleData_When_Validated_Then_ShouldReturnInvalidResult()
        {
            // Arrange
            var sale = new Sale("", DateTime.Now, null, null, false);

            // Act
            var validationResult = sale.Validate();

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(validationResult.Errors);
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
