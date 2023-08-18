namespace ProductMicroservice.Models;

public partial class PurchaseDetail
{
    public string Id { get; set; } = null!;

    public string PurchaseId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;
}
