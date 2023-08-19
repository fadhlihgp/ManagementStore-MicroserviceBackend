namespace PurchaseMicroservice.ViewModels;

public class PurchaseDetailRequestDto
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public Decimal Price { get; set; }
}