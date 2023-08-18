namespace ProductMicroservice.Models;

public partial class Delivery
{
    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public string CustomerId { get; set; } = null!;

    public bool IsDelivered { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();

    public virtual Store Store { get; set; } = null!;
}
