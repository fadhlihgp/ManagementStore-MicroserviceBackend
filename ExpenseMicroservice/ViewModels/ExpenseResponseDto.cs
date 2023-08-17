namespace ExpenseMicroservice.ViewModels;

public class ExpenseResponseDto
{
    public string Id { get; set; }
    public string Date { get; set; }
    public Decimal TotalExpense { get; set; }
}