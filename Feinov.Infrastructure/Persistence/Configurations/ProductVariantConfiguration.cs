using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("product_variants");

        builder.HasKey(variant => variant.Id);

        builder.Property(variant => variant.Id)
            .HasColumnName("variant_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(variant => variant.ProductId)
            .HasColumnName("product_id");

        builder.Property(variant => variant.Sku)
            .HasColumnName("sku")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(variant => variant.VariantName)
            .HasColumnName("variant_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(variant => variant.SizeMl)
            .HasColumnName("size_ml");

        builder.Property(variant => variant.PackSize)
            .HasColumnName("pack_size")
            .HasDefaultValue(1);

        builder.Property(variant => variant.WeightGrams)
            .HasColumnName("weight_grams")
            .HasColumnType("decimal(10,2)");

        builder.Property(variant => variant.Mrp)
            .HasColumnName("mrp")
            .HasColumnType("decimal(18,2)");

        builder.Property(variant => variant.Barcode)
            .HasColumnName("barcode")
            .HasMaxLength(100);

        builder.Property(variant => variant.IsDefault)
            .HasColumnName("is_default")
            .HasDefaultValue(false);

        builder.Property(variant => variant.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(variant => variant.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(variant => variant.UpdatedDate)
            .HasColumnName("updated_date");

        builder.HasOne(variant => variant.Product)
            .WithMany(product => product.Variants)
            .HasForeignKey(variant => variant.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_product_variants_product");

        builder.HasIndex(variant => variant.Sku)
            .IsUnique()
            .HasDatabaseName("uq_product_variants_sku");
    }
}
