using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Models;

[Keyless]
[Table("inventory")]
public partial class Inventory
{
    [Column("variant_id")]
    public Guid VariantId { get; set; }

    [Column("total_stock")]
    public int TotalStock { get; set; }

    [Column("reserved_stock")]
    public int ReservedStock { get; set; }

    [Column("available_stock")]
    public int AvailableStock { get; set; }

    [Column("reorder_level")]
    public int ReorderLevel { get; set; }

    [Column("last_stock_updated", TypeName = "timestamp without time zone")]
    public DateTime LastStockUpdated { get; set; }

    [ForeignKey("VariantId")]
    public virtual ProductVariant Variant { get; set; } = null!;
}
