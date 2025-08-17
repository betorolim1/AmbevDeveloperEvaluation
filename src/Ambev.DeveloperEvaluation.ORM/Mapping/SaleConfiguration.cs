using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SaleNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Date)
                   .HasColumnType("timestamp")
                   .IsRequired();

            builder.Property(x => x.Cancelled)
                   .IsRequired();

            builder.HasMany(x => x.Items)
                   .WithOne()
                   .HasForeignKey("SaleId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(x => x.Customer, c =>
            {
                c.Property(p => p.CustomerId).HasColumnName("CustomerId");
                c.Property(p => p.CustomerName).HasColumnName("CustomerName").HasMaxLength(100);
            });

            builder.OwnsOne(x => x.Branch, c =>
            {
                c.Property(p => p.BranchId).HasColumnName("BranchId");
                c.Property(p => p.BranchName).HasColumnName("BranchName").HasMaxLength(100);
            });

            builder.HasMany(x => x.Items)
               .WithOne()
               .HasForeignKey(i => i.SaleId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
