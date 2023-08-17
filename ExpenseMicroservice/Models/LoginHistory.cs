﻿using ExpenseMicroservice.Models;

namespace ExpenseMicroservice.Models;

public partial class LoginHistory
{
    public string Id { get; set; } = null!;

    public string AccountId { get; set; } = null!;

    public DateTime LastLogin { get; set; }

    public virtual Account Account { get; set; } = null!;
}
