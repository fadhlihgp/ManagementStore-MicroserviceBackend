using PurchaseMicroservice.ViewModels;

namespace PurchaseMicroservice.Repositories;

public interface IPurchaseRepository
{
    Task CreatePurchase(string storeId, string roleId, string purchaseTypeId, PurchaseRequestDto requestDto);
    Task<IEnumerable<PurchaseResponseDto>> ListPurchase(string storeId, int? month, int? year);
    Task<IEnumerable<PurchaseDetailResponseDto>> GetPurchaseDetail(string purchaseId);
}