using AccountAuthMicroservice.Entities;
using AccountAuthMicroservice.Exceptions;
using AccountAuthMicroservice.Repositories;
using AccountAuthMicroservice.Repositories.Interface;
using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;

namespace AccountAuthMicroservice.Services.Impl;

public class LoginHistoryService : ILoginHistoryService
{
    private IPersistence _persistence;
    private IRepository<LoginHistory> _loginHistoryRepository;

    public LoginHistoryService(IPersistence persistence, IRepository<LoginHistory> loginHistoryRepository)
    {
        _persistence = persistence;
        _loginHistoryRepository = loginHistoryRepository;
    }

    public async Task UpsertLoginHistory(string accountId)
    {
        var findById = await _loginHistoryRepository.Find(l => l.AccountId.Equals(accountId));
        var loginHistory = new LoginHistory();
        if (findById == null)
        {
            try
            {
                await _persistence.BeginTransactionAsync();

                loginHistory.Id = Guid.NewGuid().ToString();
                loginHistory.AccountId = accountId;
                loginHistory.LastLogin = DateTime.Now;
                
                await _loginHistoryRepository.Save(loginHistory);

                await _persistence.CommitTransactionAsync();
                await _persistence.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await _persistence.RollbackTransactionAsync();
                throw new Exception(e.Message);
            }
        }
        else
        {
            try
            {
                await _persistence.BeginTransactionAsync();
                
                findById.LastLogin = DateTime.Now;
                _loginHistoryRepository.Update(findById);

                await _persistence.CommitTransactionAsync();
                await _persistence.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await _persistence.RollbackTransactionAsync();
                throw new Exception(e.Message);
            }
        }
    }

    public async Task<List<LoginHistoryResponseDto>> ListLoginHistoryByStoreId(string storeId, string roleId)
    {
        if (!roleId.Equals("2")) throw new UnauthorizedException("Akses ditolak");
        
        var loginHistories = await _loginHistoryRepository.FindAll(x => x.Account.Member.StoreId.Equals(storeId),
            new[] { "Account.Member.Store", "Account", "Account.Role" });

        List<LoginHistoryResponseDto> result = new List<LoginHistoryResponseDto>();
        
        foreach (var loginHistory in loginHistories)
        {
            result.Add(new LoginHistoryResponseDto
            {
                Id = loginHistory.Id.ToString(),
                Username = loginHistory.Account.UserName,
                Role = loginHistory.Account.Role.Name,
                Store = loginHistory.Account.Member.Store.Name,
                LastLogin = loginHistory.LastLogin.ToString("dd-MM-yyyy HH:mm:ss")
            });
        }
        
        return result;
    }

    public async Task<List<LoginHistoryResponseDto>> ListLoginHistory(string roleId)
    {
        if (!roleId.Equals("1")) throw new UnauthorizedException("Akses ditolak");
        
        var loginHistories = await _loginHistoryRepository.FindAll(
            new[] { "Account.Member.Store", "Account", "Account.Role" });

        List<LoginHistoryResponseDto> result = new List<LoginHistoryResponseDto>();
        
        foreach (var loginHistory in loginHistories)
        {
            result.Add(new LoginHistoryResponseDto
            {
                Id = loginHistory.Id.ToString(),
                Username = loginHistory.Account.UserName,
                Role = loginHistory.Account.Role.Name,
                Store = loginHistory.Account.Member.Store.Name
            });
        }
        
        return result;
    }
}