using DebtMicroservice.Utilities;
using DebtMicroservice.ViewModels;

namespace DebtMicroservice.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private IBaseService _baseService;

    public PurchaseRepository(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto> CreateNewPurchase(PurchaseRequestDto purchaseRequestDto)
    {
        var request = new RequestDto
        {
            ApiType = ApiType.POST,
            Url = $"{ApiUrl.PurchaseUrl}/create/{3}",
            Data = purchaseRequestDto
        };

        var responseDto = await _baseService.SendAsync(request);
        return responseDto;
    }
}