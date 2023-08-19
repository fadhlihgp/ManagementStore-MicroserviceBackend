using PurchaseMicroservice.ViewModels;

namespace PurchaseMicroservice.Repositories;

public interface IProductService
{
    Task<ResponseDto> ReduceStock(AddReduceRequestDto requestDto);
}