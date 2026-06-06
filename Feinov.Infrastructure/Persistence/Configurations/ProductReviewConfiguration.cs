using Feinov.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feinov.Infrastructure.Persistence.Configurations;

public sealed class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.ToTable("product_reviews");

        builder.HasKey(review => review.Id);

        builder.Property(review => review.Id)
            .HasColumnName("review_id")
            .HasDefaultValueSql("NEWID()");

        builder.Property(review => review.ProductId)
            .HasColumnName("product_id");

        builder.Property(review => review.UserId)
            .HasColumnName("user_id");

        builder.Property(review => review.Rating)
            .HasColumnName("rating");

        builder.Property(review => review.ReviewTitle)
            .HasColumnName("review_title")
            .HasMaxLength(200);

        builder.Property(review => review.ReviewText)
            .HasColumnName("review_text");

        builder.Property(review => review.IsApproved)
            .HasColumnName("is_approved")
            .HasDefaultValue(false);

        builder.Property(review => review.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(review => review.Product)
            .WithMany(product => product.Reviews)
            .HasForeignKey(review => review.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(review => review.User)
            .WithMany(user => user.Reviews)
            .HasForeignKey(review => review.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
