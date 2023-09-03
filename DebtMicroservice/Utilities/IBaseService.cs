using DebtMicroservice.ViewModels;

namespace DebtMicroservice.Utilities;

public interface IBaseService
{
    Task<ResponseDto>? SendAsync(RequestDto requestDto);
}

public enum ApiType
{
    GET, POST, PUT, DELETE
}