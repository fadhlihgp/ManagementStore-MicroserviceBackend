namespace PurchaseMicroservice.ViewModels;

public class PurchaseResponseDto
{
    public string PurchaseId { get; set; }
    public DateTime Date { get; set; }
    public string PurchaseTypeName { get; set; }
    public Decimal Money { get; set; }
    public Decimal TotalPrice { get; set; }
    
}