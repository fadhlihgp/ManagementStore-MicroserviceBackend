namespace ExpenseMicroservice.ViewModels;

public class ExpenseDetailCreateDto
{
    public string ExpenseId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Decimal Price { get; set; }
}