using PurchaseMicroservice.Utilities;

namespace PurchaseMicroservice.ViewModels;

public class RequestDto
{
    public ApiType ApiType { get; set; }
    public string Url { get; set; }
    public object Data { get; set; }
}