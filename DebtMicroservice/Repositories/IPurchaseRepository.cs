using DebtMicroservice.ViewModels;

namespace DebtMicroservice.Repositories;

public interface IPurchaseRepository
{
    Task<ResponseDto> CreateNewPurchase(PurchaseRequestDto purchaseRequestDto);
}