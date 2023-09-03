namespace DebtMicroservice.ViewModels;

public class PurchaseRequestDto
{
    public Decimal Money { get; set; }
    public IEnumerable<PurchaseDetailRequestDto> PurchaseDetails { get; set; }
}