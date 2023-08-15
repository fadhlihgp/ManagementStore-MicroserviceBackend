using System;
using System.Collections.Generic;

namespace AccountAuth.Entities;

public partial class Image
{
    public string Id { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
