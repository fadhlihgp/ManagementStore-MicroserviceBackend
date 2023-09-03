namespace DebtMicroservice.Models;

public partial class Image
{
    public string Id { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
