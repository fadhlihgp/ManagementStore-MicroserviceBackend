namespace DebtMicroservice.ViewModels;

public class DebtCreateDto
{
    public string CustomerId { get; set;}
    public int Month { get; set; }
    public int Year { get; set; }
    public IEnumerable<DebtDetailRequestDto> DebtDetails { get; set; }
}