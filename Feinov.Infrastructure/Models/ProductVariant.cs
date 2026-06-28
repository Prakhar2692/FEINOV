using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Table("product_variants")]
[Index("Sku", Name = "uq_product_variants_sku", IsUnique = true)]
public partial class ProductVariant
{
    [Key]
    [Column("variant_id")]
    public Guid VariantId { get; set; }

    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("sku")]
    [StringLength(100)]
    public string Sku { get; set; } = null!;

    [Column("variant_name")]
    [StringLength(200)]
    public string VariantName { get; set; } = null!;

    [Column("size_ml")]
    public int? SizeMl { get; set; }

    [Column("pack_size")]
    public int PackSize { get; set; }

    [Column("weight_grams")]
    [Precision(10, 2)]
    public decimal? WeightGrams { get; set; }

    [Column("mrp")]
    [Precision(18, 2)]
    public decimal Mrp { get; set; }

    [Column("barcode")]
    [StringLength(100)]
    public string? Barcode { get; set; }

    [Column("is_default")]
    public bool IsDefault { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_date", TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("updated_date", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedDate { get; set; }

    [InverseProperty("Variant")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("Variant")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("ProductId")]
    [InverseProperty("ProductVariants")]
    public virtual Product Product { get; set; } = null!;
}
