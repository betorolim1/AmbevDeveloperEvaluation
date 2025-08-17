using Ambev.DeveloperEvaluation.Domain.Entities.ExternalIdentities;

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

        public Sale(string saleNumber, DateTime date, CustomerExternalIdentity customer, BranchExternalIdentity branch)
        {
            Id = Guid.NewGuid();
            SaleNumber = saleNumber;
            Date = date;
            Customer = customer;
            Branch = branch;
            Cancelled = false;
        }

        public void AddItem(ProductExternalIdentity product, int quantity)
        {
            var item = new SaleItem(product, quantity);
            _items.Add(item);
        }

        public void Cancel() => Cancelled = true;
    }
}
