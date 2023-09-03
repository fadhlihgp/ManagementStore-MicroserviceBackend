﻿namespace DebtMicroservice.Models;

public partial class Debt
{
    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public string CustomerId { get; set; } = null!;

    public bool IsPaid { get; set; }
    
    public decimal? Money { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual IEnumerable<DebtDetail>? DebtDetails { get; set; } = new List<DebtDetail>();

    public virtual Store Store { get; set; } = null!;
}
