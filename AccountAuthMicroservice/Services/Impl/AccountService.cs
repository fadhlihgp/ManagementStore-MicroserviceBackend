using AccountAuthMicroservice.Config;
using AccountAuthMicroservice.Exceptions;
using AccountAuthMicroservice.Models;
using AccountAuthMicroservice.Repositories.Interface;
using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;
using Microsoft.IdentityModel.Tokens;

namespace AccountAuthMicroservice.Services.Impl;

public class AccountService : IAccountService
{
    private IRepository<Account> _repository;
    private IPersistence _persistence;

    public AccountService(IRepository<Account> repository, IPersistence persistence)
    {
        _repository = repository;
        _persistence = persistence;
    }

    // ================== List akun yang hanya dapat diakses oleh super admin dan menampilkan semua akun =================
    public async Task<IEnumerable<AccountResponseDto>> ListAccount(string roleId)
    {
        if (!roleId.Equals("1")) throw new UnauthorizedException("Akses ditolak");

        IEnumerable<AccountResponseDto> result = new List<AccountResponseDto>();
        var listAccount = await _repository.FindAll(new []{"Role", "Member.Store", "Member"});
        result = listAccount.Select(account => new AccountResponseDto
        {
            Id = account.Id,
            UserName = account.UserName,
            Email = account.Email,
            NoHp = account.NoHp,
            MemberId = account.MemberId,
            MemberName = account.Member.Name,
            StoreName = account.Member.Store.Id,
            RoleId = account.Role.Id,
            Role = account.Role.Name
        });
        return result;
    }

    // ===================== List akun yg diakses oleh owner dan data terfilter by storeId yang login
    public async Task<IEnumerable<AccountResponseDto>> ListAccountBaseStoreIds(string roleId, string storeId)
    {
        if (!roleId.Equals("2")) throw new UnauthorizedException("Akses ditolak");

        var accountList = await _repository.FindAll(a => a.Member.Store.Id.Equals(storeId),
            new[] { "Role", "Member.Store", "Member" });
        IEnumerable<AccountResponseDto> result = new List<AccountResponseDto>();

        result = accountList.Select(account => new AccountResponseDto
        {
            Id = account.Id,
            UserName = account.UserName,
            Email = account.Email,
            NoHp = account.NoHp,
            MemberId = account.MemberId,
            MemberName = account.Member.Name,
            StoreName = account.Member.Store.Name,
            RoleId = account.RoleId,
            Role = account.Role.Name
        });
        return result;
    }

    // ======================= Update Account =================
    public async Task UpdateAccount(string id, AccountRequestDto createRequestDto)
    {
        var findById = await _repository.FindById(id);
        if (findById == null) throw new UnauthorizedException("Akses ditolak");
        
        try
        {
            await _persistence.BeginTransactionAsync();
            
            findById.UserName = createRequestDto.UserName;
            findById.Email = createRequestDto.Email;
            findById.MemberId = createRequestDto.MemberId;
            findById.NoHp = createRequestDto.NoHp;
            findById.RoleId = createRequestDto.RoleId;
            if (!createRequestDto.Password.IsNullOrEmpty())
            {
                findById.Password = BCrypt.Net.BCrypt.HashPassword(createRequestDto.Password);
            }

            _repository.Update(findById);
            await _persistence.CommitTransactionAsync();
            await _persistence.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _persistence.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    // ========================== Delete Account =======================
    public async Task DeleteAccount(string id)
    {
        var account = await _repository.FindById(id);
        if ( account == null) throw new NotFoundException("Akses ditolak");

        try
        {
            await _persistence.BeginTransactionAsync();

            _repository.Delete(account);

            await _persistence.CommitTransactionAsync();
            await _persistence.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _persistence.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    // ============================ Create Account ============================
    public async Task CreateAccount(string roleId, AccountRequestDto create)
    {
        if (roleId.Equals("3")) throw new UnauthorizedException("Akses ditolak");

        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            UserName = create.UserName,
            Email = create.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(create.Password),
            NoHp = create.NoHp,
            RoleId = create.RoleId,
            MemberId = create.MemberId,
            IsActive = true
        };
        
        try
        {
            await _persistence.BeginTransactionAsync();

            await _repository.Save(account);

            await _persistence.CommitTransactionAsync();
            await _persistence.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _persistence.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    // ========================== Account by id =========================
    public async Task<AccountResponseDto> AccountById(string accountId)
    {
        var account = await _repository.Find(a => a.Id == accountId, 
           new []{"Role", "Member.Store", "Member"});

        if (account == null) throw new NotFoundException("Data tidak ditemukan");

        var response = new AccountResponseDto
        {
            Id = account.Id,
            UserName = account.UserName,
            Email = account.Email,
            NoHp = account.NoHp,
            MemberId = account.MemberId,
            MemberName = account.Member.Name,
            StoreName = account.Member.Store.Name,
            RoleId = account.RoleId,
            Role = account.Role.Name
        };

        return response;
    }
}