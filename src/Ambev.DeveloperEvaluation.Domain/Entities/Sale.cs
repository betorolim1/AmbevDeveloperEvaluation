using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public CustomerExternalIdentity Customer { get; set; }
        public BranchExternalIdentity Branch { get; set; }
        public bool Cancelled { get; set; }

        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items;

        public decimal TotalAmount => _items.Sum(x => x.Total);

        public void AddItem(ProductExternalIdentity product, int quantity)
        {
            var item = new SaleItem(product, quantity);
            _items.Add(item);
        }

        public void ClearItems()
        {
            _items.Clear();
        }

        public ValidationResultDetail Validate()
        {
            var validator = new SaleValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
