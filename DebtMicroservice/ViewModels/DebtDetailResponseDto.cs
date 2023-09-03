namespace DebtMicroservice.ViewModels;

public class DebtDetailResponseDto
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime Date { get; set; }
}