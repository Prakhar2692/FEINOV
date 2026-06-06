using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("product_images");

        builder.HasKey(image => image.Id);

        builder.Property(image => image.Id)
            .HasColumnName("image_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(image => image.ProductId)
            .HasColumnName("product_id");

        builder.Property(image => image.ImageUrl)
            .HasColumnName("image_url")
            .IsRequired();

        builder.Property(image => image.DisplayOrder)
            .HasColumnName("display_order")
            .HasDefaultValue(1);

        builder.Property(image => image.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(image => image.Product)
            .WithMany(product => product.Images)
            .HasForeignKey(image => image.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_product_images_product");

        builder.HasIndex(image => image.ProductId)
            .HasDatabaseName("idx_product_images_product");
    }
}
