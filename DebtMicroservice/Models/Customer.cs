﻿namespace DebtMicroservice.Models;

public partial class Customer
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NoHp { get; set; } = null!;

    public string Address { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public string StoreId { get; set; } = null!;

    public virtual IEnumerable<Debt>? Debts { get; set; } = new List<Debt>();

    public virtual IEnumerable<Delivery>? Deliveries { get; set; } = new List<Delivery>();

    public virtual Store Store { get; set; } = null!;
}