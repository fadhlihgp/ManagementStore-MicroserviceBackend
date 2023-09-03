namespace DebtMicroservice.Models;

public partial class DebtDetail
{
    public string Id { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string DebtId { get; set; } = null!;
    
    public string? Description { get; set; }

    public DateTime Date { get; set; }

    public virtual Debt Debt { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
