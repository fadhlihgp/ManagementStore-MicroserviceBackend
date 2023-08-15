using System;
using System.Collections.Generic;

namespace AccountAuth.Entities;

public partial class DeliveryDetail
{
    public string Id { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string DeliveryId { get; set; } = null!;

    public virtual Delivery Delivery { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
