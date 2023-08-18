namespace ProductMicroservice.Models;

public partial class Purchase
{
    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public string PurchaseTypeId { get; set; } = null!;

    public decimal Money { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual PurchaseType PurchaseType { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
