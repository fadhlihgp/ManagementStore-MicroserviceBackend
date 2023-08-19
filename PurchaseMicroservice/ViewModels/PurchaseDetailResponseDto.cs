namespace PurchaseMicroservice.ViewModels;

public class PurchaseDetailResponseDto
{
    public string PurchaseDetailId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public Decimal Price { get; set; }
    public Decimal TotalPrice { get; set; }
}