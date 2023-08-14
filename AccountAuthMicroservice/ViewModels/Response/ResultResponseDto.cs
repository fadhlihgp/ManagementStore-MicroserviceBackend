namespace AccountAuthMicroservice.ViewModels.Response;

public class ResultResponseDto
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}