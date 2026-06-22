using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

public partial class Context : DbContext
{
    public Context()
    {
    }

    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderAddress> OrderAddresses { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OtpTransaction> OtpTransactions { get; set; }

    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VariantDiscount> VariantDiscounts { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("carts_pkey");

            entity.Property(e => e.CartId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.User).WithMany(p => p.Carts).HasConstraintName("fk_cart_user");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("cart_items_pkey");

            entity.Property(e => e.CartItemId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems).HasConstraintName("fk_cart_item_cart");

            entity.HasOne(d => d.Variant).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cart_item_variant");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_pkey");

            entity.Property(e => e.CategoryId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("customer_addresses_pkey");

            entity.Property(e => e.AddressId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Country).HasDefaultValueSql("'India'::character varying");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDefault).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithMany(p => p.CustomerAddresses).HasConstraintName("fk_customer_addresses_user");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.Property(e => e.AvailableStock).HasDefaultValue(0);
            entity.Property(e => e.LastStockUpdated).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ReorderLevel).HasDefaultValue(10);
            entity.Property(e => e.ReservedStock).HasDefaultValue(0);
            entity.Property(e => e.TotalStock).HasDefaultValue(0);

            entity.HasOne(d => d.Variant).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inventory_variant");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.Property(e => e.OrderId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_customer");
        });

        modelBuilder.Entity<OrderAddress>(entity =>
        {
            entity.HasKey(e => e.OrderAddressId).HasName("order_addresses_pkey");

            entity.Property(e => e.OrderAddressId).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Order).WithOne(p => p.OrderAddress).HasConstraintName("fk_order_address_order");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("order_items_pkey");

            entity.Property(e => e.OrderItemId).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("fk_order_items_order");

            entity.HasOne(d => d.Variant).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_items_variant");
        });

        modelBuilder.Entity<OtpTransaction>(entity =>
        {
            entity.HasKey(e => e.OtpTransactionId).HasName("otp_transactions_pkey");

            entity.Property(e => e.OtpTransactionId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsUsed).HasDefaultValue(false);
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasKey(e => e.PaymentTransactionId).HasName("payment_transactions_pkey");

            entity.Property(e => e.PaymentTransactionId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Order).WithMany(p => p.PaymentTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_transaction_order");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pkey");

            entity.Property(e => e.ProductId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_products_subcategory");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("product_images_pkey");

            entity.Property(e => e.ImageId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(1);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages).HasConstraintName("fk_product_images_product");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("product_reviews_pkey");

            entity.Property(e => e.ReviewId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsApproved).HasDefaultValue(false);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.VariantId).HasName("product_variants_pkey");

            entity.Property(e => e.VariantId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.PackSize).HasDefaultValue(1);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants).HasConstraintName("fk_product_variants_product");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.Property(e => e.RoleId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.SubcategoryId).HasName("subcategories_pkey");

            entity.Property(e => e.SubcategoryId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Category).WithMany(p => p.Subcategories)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_subcategories_category");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.Property(e => e.UserId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsEmailVerified).HasDefaultValue(false);
            entity.Property(e => e.IsMobileVerified).HasDefaultValue(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_users_role");
        });

        modelBuilder.Entity<VariantDiscount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("variant_discounts_pkey");

            entity.Property(e => e.DiscountId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("wishlists_pkey");

            entity.Property(e => e.WishlistId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
