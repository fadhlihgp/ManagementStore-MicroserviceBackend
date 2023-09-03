namespace DebtMicroservice.ViewModels;

public class ErrorResponseDto
{
    public int StatusCode { get; set;}
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}