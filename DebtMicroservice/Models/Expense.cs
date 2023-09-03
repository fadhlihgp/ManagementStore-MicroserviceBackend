namespace DebtMicroservice.Models;

public partial class Expense
{
    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual IEnumerable<ExpenseDetail>? ExpenseDetails { get; set; } = new List<ExpenseDetail>();

    public virtual Store Store { get; set; } = null!;
}
