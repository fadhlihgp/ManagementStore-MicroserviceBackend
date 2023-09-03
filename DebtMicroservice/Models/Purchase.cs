namespace DebtMicroservice.Models;

public partial class Purchase
{
    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public string PurchaseTypeId { get; set; } = null!;

    public decimal Money { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual IEnumerable<PurchaseDetail>? PurchaseDetails { get; set; }

    public virtual PurchaseType PurchaseType { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
