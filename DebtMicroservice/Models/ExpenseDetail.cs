namespace DebtMicroservice.Models;

public partial class ExpenseDetail
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTime Date { get; set; }

    public string? ExpenseId { get; set; }

    public virtual Expense? Expense { get; set; }
}
