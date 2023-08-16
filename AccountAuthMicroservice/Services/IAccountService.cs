using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;

namespace AccountAuthMicroservice.Services;

public interface IAccountService
{
    Task<IEnumerable<AccountResponseDto>> ListAccount(string roleId);
    Task<IEnumerable<AccountResponseDto>> ListAccountBaseStoreIds(string roleId, string storeId);
    Task UpdateAccount(string id, AccountRequestDto createRequestDto);
    Task DeleteAccount(string id);
    Task CreateAccount(string roleId, AccountRequestDto create);
    Task<AccountResponseDto> AccountById(string accountId);
}