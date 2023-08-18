namespace ProductMicroservice.ViewModels;

public class ProductRequestDto
{
    public string ProductCode { get; set; }
    public string Name { get; set;}
    public string Description { get; set;}
    public Decimal Price { get; set;}
    public int Stock { get; set;}
    public string Unit { get; set;}
    public IEnumerable<ImageRequestDto>? Images { get; set; }
}