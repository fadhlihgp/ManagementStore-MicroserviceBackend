using System;
using System.Collections.Generic;

namespace AccountAuthMicroservice.Models;

public partial class Customer
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NoHp { get; set; } = null!;

    public string Address { get; set; } = null!;

    public byte IsDeleted { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual ICollection<Debt> Debts { get; set; } = new List<Debt>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual Store Store { get; set; } = null!;
}
