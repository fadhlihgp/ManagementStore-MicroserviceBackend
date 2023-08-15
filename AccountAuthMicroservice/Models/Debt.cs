using System;
using System.Collections.Generic;

namespace AccountAuthMicroservice.Models;

public partial class Debt
{
    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public string CustomerId { get; set; } = null!;

    public byte IsPaid { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<DebtDetail> DebtDetails { get; set; } = new List<DebtDetail>();

    public virtual Store Store { get; set; } = null!;
}
