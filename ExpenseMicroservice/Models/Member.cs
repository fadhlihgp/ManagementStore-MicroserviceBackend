﻿using ExpenseMicroservice.Models;

namespace ExpenseMicroservice.Models;

public partial class Member
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual Store Store { get; set; } = null!;
}
