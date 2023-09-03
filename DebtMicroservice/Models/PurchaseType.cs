namespace DebtMicroservice.Models;

public partial class PurchaseType
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual IEnumerable<Purchase>? Purchases { get; set; } = new List<Purchase>();
}
