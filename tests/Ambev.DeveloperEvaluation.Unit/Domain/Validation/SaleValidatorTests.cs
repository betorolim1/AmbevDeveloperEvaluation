using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    public class SaleValidatorTests
    {
        private readonly SaleValidator _validator;

        public SaleValidatorTests()
        {
            _validator = new SaleValidator();
        }

        [Fact(DisplayName = "SaleValidator - Given valid sale Then validation succeeds")]
        public void Validate_Given_ValidSale_Then_ShouldNotHaveErrors()
        {
            var sale = SaleTestData.GenerateValidSale();

            var result = _validator.TestValidate(sale);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory(DisplayName = "SaleValidator - Invalid SaleNumber")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Lorem Ipsum is simply dummy text of the printing an")] // 51 caracters
        public void Validate_Given_InvalidSaleNumber_Then_ShouldHaveError(string saleNumber)
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.SaleNumber = saleNumber;

            var result = _validator.TestValidate(sale);
            result.ShouldHaveValidationErrorFor(s => s.SaleNumber);
        }

        [Fact(DisplayName = "SaleValidator - Missing Date")]
        public void Validate_Given_MissingDate_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.Date = default;

            var result = _validator.TestValidate(sale);
            result.ShouldHaveValidationErrorFor(s => s.Date);
        }

        [Fact(DisplayName = "SaleValidator - Missing Customer info")]
        public void Validate_Given_MissingCustomer_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.Customer = null;

            var result = _validator.TestValidate(sale);
            result.ShouldHaveValidationErrorFor(s => s.Customer);
        }

        [Fact(DisplayName = "SaleValidator - Missing Customer Id")]
        public void Validate_Given_MissingCustomerId_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.Customer.CustomerId = Guid.Empty;

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(s => s.Customer.CustomerId);
        }

        [Fact(DisplayName = "SaleValidator - Missing Customer Name")]
        public void Validate_Given_MissingCustomerName_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.Customer.CustomerName = null;

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(s => s.Customer.CustomerName);
        }

        [Fact(DisplayName = "SaleValidator - Missing Branch info")]
        public void Validate_Given_MissingBranch_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.Branch = null;

            var result = _validator.TestValidate(sale);
            result.ShouldHaveValidationErrorFor(s => s.Branch);
        }

        [Fact(DisplayName = "SaleValidator - Missing Branch Id")]
        public void Validate_Given_MissingBranchId_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.Branch.BranchId = Guid.Empty;

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(s => s.Branch.BranchId);
        }

        [Fact(DisplayName = "SaleValidator - Missing Branch Name")]
        public void Validate_Given_MissingBranchName_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.Branch.BranchName = null;

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(s => s.Branch.BranchName);
        }

        [Fact(DisplayName = "SaleValidator - Items with invalid product id")]
        public void Validate_Given_ItemsWithInvalidProductId_Then_ShouldHaveError()
        {
            var product = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();

            var sale = SaleTestData.GenerateValidSale();
            sale.AddItem(product, 1);
            sale.Items.First().Product.ProductId = Guid.Empty;

            var result = _validator.TestValidate(sale);
            result.ShouldHaveValidationErrorFor("Items[0].Product.ProductId");
        }

        [Fact(DisplayName = "SaleValidator - Items with invalid product name")]
        public void Validate_Given_ItemsWithInvalidProductName_Then_ShouldHaveError()
        {
            var product = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();

            var sale = SaleTestData.GenerateValidSale();
            sale.AddItem(product, 1);
            sale.Items.First().Product.ProductName = null;

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor("Items[0].Product.ProductName");
        }

        [Fact(DisplayName = "SaleValidator - Items with invalid product price")]
        public void Validate_Given_ItemsWithInvalidProductPrice_Then_ShouldHaveError()
        {
            var product = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();

            var sale = SaleTestData.GenerateValidSale();
            sale.AddItem(product, 1);
            sale.Items.First().Product.ProductPrice = 0;

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor("Items[0].Product.ProductPrice");
        }

        [Theory(DisplayName = "SaleValidator - Invalid Quantity in Items")]
        [InlineData(0)]
        [InlineData(21)]
        public void Validate_Given_InvalidItemQuantity_Then_ShouldHaveError(int quantity)
        {
            var product = ProductExternalIdentityTestData.GenerateValidProductExternalIdentity();

            var sale = SaleTestData.GenerateValidSale();
            sale.AddItem(product, quantity);

            var result = _validator.TestValidate(sale);
            result.ShouldHaveValidationErrorFor("Items[0].Quantity");
        }
    }
}
