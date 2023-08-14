using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;

namespace AccountAuthMicroservice.Services;

public interface IStoreService
{
    Task CreateStoreAsync(StoreRequestDto storeRequestDto);
    Task UpdateStoreAsync(StoreRequestDto storeRequestDto, string roleId);
    Task<IEnumerable<StoreResponseDto>> ListStoreAsync(string roleId);
    Task<StoreResponseDto> FindStoreById(string storeId);
}