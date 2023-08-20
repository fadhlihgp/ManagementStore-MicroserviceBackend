using PurchaseMicroservice.Utilities;
using PurchaseMicroservice.ViewModels;

namespace PurchaseMicroservice.Repositories;

public class ProductService : IProductService
{
    private IBaseService _baseService;

    public ProductService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto> ReduceStock(AddReduceRequestDto requestDto)
    {

        var request = new RequestDto
        {
            ApiType = ApiType.PUT,
            Url = $"{ApiUrl.ProductUrl}/api/product/reduce-stock",
            Data = requestDto
        };

        var responseDto = await _baseService.SendAsync(request);
        return responseDto;
    }
}