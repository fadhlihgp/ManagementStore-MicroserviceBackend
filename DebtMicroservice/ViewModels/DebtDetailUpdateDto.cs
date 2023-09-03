namespace DebtMicroservice.ViewModels;

public class DebtDetailUpdateDto
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set;}
    public decimal Price { get; set; }
}