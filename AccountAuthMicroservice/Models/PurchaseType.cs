using System;
using System.Collections.Generic;

namespace AccountAuthMicroservice.Models;

public partial class PurchaseType
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
