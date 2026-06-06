using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class SubcategoryConfiguration : IEntityTypeConfiguration<Subcategory>
{
    public void Configure(EntityTypeBuilder<Subcategory> builder)
    {
        builder.ToTable("subcategories");

        builder.HasKey(subcategory => subcategory.Id);

        builder.Property(subcategory => subcategory.Id)
            .HasColumnName("subcategory_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(subcategory => subcategory.CategoryId)
            .HasColumnName("category_id");

        builder.Property(subcategory => subcategory.SubcategoryName)
            .HasColumnName("subcategory_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(subcategory => subcategory.Description)
            .HasColumnName("description");

        builder.Property(subcategory => subcategory.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(subcategory => subcategory.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(subcategory => subcategory.UpdatedDate)
            .HasColumnName("updated_date");

        builder.HasOne(subcategory => subcategory.Category)
            .WithMany(category => category.Subcategories)
            .HasForeignKey(subcategory => subcategory.CategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_subcategories_category");

        builder.HasIndex(subcategory => subcategory.CategoryId)
            .HasDatabaseName("idx_subcategories_category");

        builder.HasIndex(subcategory => new { subcategory.CategoryId, subcategory.SubcategoryName })
            .IsUnique()
            .HasDatabaseName("idx_subcategory_name_per_category");
    }
}
