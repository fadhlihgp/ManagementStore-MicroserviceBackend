namespace ProductMicroservice.ViewModels;

public class ProductResponseDto
{
    public string Id { get; set; }
    public string ProductCode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Decimal Price { get; set; }
    public int Stock { get; set; }
    public string CreatedAt { get; set; }
    public string CreatedName { get; set; }
    public string EditedAt { get; set; }
    public string EditedName { get; set; }
    public string Unit { get; set; }
    public IEnumerable<ImageResponseDto>? Images { get; set;}
}