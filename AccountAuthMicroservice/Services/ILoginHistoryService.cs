using AccountAuthMicroservice.Models;
using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;

namespace AccountAuthMicroservice.Services;

public interface ILoginHistoryService 
{
    Task UpsertLoginHistory(string accountId);
    Task<List<LoginHistoryResponseDto>> ListLoginHistoryByStoreId(string storeId, string roleId);
    Task<List<LoginHistoryResponseDto>> ListLoginHistory(string roleId);
}