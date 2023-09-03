namespace DebtMicroservice.ViewModels;

public class DebtResponseDto
{
    public string DebtId { get; set; }
    public DateTime Date { get; set; }
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public bool IsPaid { get; set; }
    public decimal Total { get; set; }
    public IEnumerable<DebtDetailResponseDto>? DebtDetails { get; set; }
}