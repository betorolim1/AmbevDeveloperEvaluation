using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(x => x.Id);

            builder.Property<Guid>("SaleId");

            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Discount).IsRequired();

            builder.OwnsOne(x => x.Product, p =>
            {
                p.Property(px => px.ProductId).HasColumnName("ProductId");
                p.Property(px => px.ProductName).HasColumnName("ProductName").HasMaxLength(100);
                p.Property(px => px.ProductPrice).HasColumnName("ProductPrice");
            });

            builder.Property(x => x.SaleId).IsRequired();
        }
    }
}
