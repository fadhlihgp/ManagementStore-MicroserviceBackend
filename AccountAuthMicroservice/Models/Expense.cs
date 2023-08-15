using System;
using System.Collections.Generic;

namespace AccountAuthMicroservice.Models;

public partial class Expense
{
    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual ICollection<ExpenseDetail> ExpenseDetails { get; set; } = new List<ExpenseDetail>();

    public virtual Store Store { get; set; } = null!;
}
