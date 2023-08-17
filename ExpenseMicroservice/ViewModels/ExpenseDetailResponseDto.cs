namespace ExpenseMicroservice.ViewModels;

public class ExpenseDetailResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Date { get; set; }
    public Decimal Price { get; set; }
}