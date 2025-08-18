using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; private set; }
        public string SaleNumber { get; private set; }
        public DateTime Date { get; private set; }
        public CustomerExternalIdentity Customer { get; private set; }
        public BranchExternalIdentity Branch { get; private set; }
        public bool Cancelled { get; private set; }

        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items;

        public decimal TotalAmount => _items.Sum(x => x.Total);

        public Sale() { }

        public Sale(string saleNumber, DateTime date, CustomerExternalIdentity customer, BranchExternalIdentity branch, bool cancelled)
        {
            Id = Guid.NewGuid();
            SaleNumber = saleNumber;
            Date = date;
            Customer = customer;
            Branch = branch;
            Cancelled = cancelled;
        }

        public void AddItem(ProductExternalIdentity product, int quantity)
        {
            var item = new SaleItem(product, quantity);
            _items.Add(item);
        }

        public void SetCancelled(bool cancelled) => Cancelled = cancelled;

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
