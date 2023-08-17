namespace ExpenseMicroservice.ViewModels;

public class ExpenseDetailRequestDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public Decimal Price { get; set; }
    public int Month { get; set; }
    public int year { get; set;}
}