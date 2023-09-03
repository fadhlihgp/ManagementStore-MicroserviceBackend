using DebtMicroservice.Utilities;

namespace DebtMicroservice.ViewModels;

public class RequestDto
{
    public ApiType ApiType { get; set; }
    public string Url { get; set; }
    public object Data { get; set; }
}