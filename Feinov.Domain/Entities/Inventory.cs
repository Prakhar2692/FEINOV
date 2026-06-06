namespace Feinov.Domain.Entities;

public class Inventory
{
    public Guid VariantId { get; set; }

    public int TotalStock { get; set; }

    public int ReservedStock { get; set; }

    public int AvailableStock { get; set; }

    public int ReorderLevel { get; set; } = 10;

    public DateTime LastStockUpdated { get; set; }

    public ProductVariant Variant { get; set; } = null!;
}
