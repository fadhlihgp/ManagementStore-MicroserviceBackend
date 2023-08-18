namespace ProductMicroservice.Models;

public partial class Product
{
    public string Id { get; set; } = null!;

    public string ProductCode { get; set; }
    
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedAccountId { get; set; } = null!;

    public DateTime EditedAt { get; set; }

    public string EditedAccountId { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual Account CreatedAccount { get; set; } = null!;

    public virtual ICollection<DebtDetail> DebtDetails { get; set; } = new List<DebtDetail>();

    public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();

    public virtual Account EditedAccount { get; set; } = null!;

    public virtual IEnumerable<Image>? Images { get; set; } = new List<Image>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual Store Store { get; set; } = null!;
}
