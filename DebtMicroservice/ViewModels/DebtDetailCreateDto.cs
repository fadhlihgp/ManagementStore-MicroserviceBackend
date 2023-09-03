namespace DebtMicroservice.ViewModels;

public class DebtDetailCreateDto
{
    public string DebtId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set;}
    public decimal Price { get; set; }
    public string? Description { get; set; }
}