using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Id)
            .HasColumnName("product_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(product => product.SubcategoryId)
            .HasColumnName("subcategory_id");

        builder.Property(product => product.ProductName)
            .HasColumnName("product_name")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(product => product.Description)
            .HasColumnName("description");

        builder.Property(product => product.Brand)
            .HasColumnName("brand")
            .HasMaxLength(200);

        builder.Property(product => product.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(18,2)");

        builder.Property(product => product.DiscountPrice)
            .HasColumnName("discount_price")
            .HasColumnType("decimal(18,2)");

        builder.Property(product => product.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(product => product.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(product => product.UpdatedDate)
            .HasColumnName("updated_date");

        builder.HasOne(product => product.Subcategory)
            .WithMany(subcategory => subcategory.Products)
            .HasForeignKey(product => product.SubcategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_products_subcategory");

        builder.HasIndex(product => product.SubcategoryId)
            .HasDatabaseName("idx_products_subcategory");

        builder.HasIndex(product => product.ProductName)
            .HasDatabaseName("idx_products_name");
    }
}
