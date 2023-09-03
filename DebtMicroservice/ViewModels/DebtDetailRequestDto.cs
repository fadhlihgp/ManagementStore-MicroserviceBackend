namespace DebtMicroservice.ViewModels;

public class DebtDetailRequestDto
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}