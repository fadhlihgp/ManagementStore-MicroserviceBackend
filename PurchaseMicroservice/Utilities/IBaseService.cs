using PurchaseMicroservice.ViewModels;

namespace PurchaseMicroservice.Utilities;

public interface IBaseService
{
    Task<ResponseDto>? SendAsync(RequestDto requestDto);
}

public enum ApiType
{
    GET, POST, PUT, DELETE
}