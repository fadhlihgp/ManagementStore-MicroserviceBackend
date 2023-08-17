namespace ExpenseMicroservice.ViewModels;

public class ExpenseDetailUpdateRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Decimal Price { get; set; }
}