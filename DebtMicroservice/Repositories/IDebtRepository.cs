using DebtMicroservice.ViewModels;

namespace DebtMicroservice.Repositories;

public interface IDebtRepository
{
    Task CreateNewDebt(string storeId, DebtCreateDto createDto);
    Task<IEnumerable<DebtResponseDto>> ListDebt(string storeId);
    Task<DebtResponseDto> GetDebtById(string debtId);
    Task DeleteDebtById(string debtId);
    Task PayDebt(PayDebtDto debtId);
    Task AddDebtNextMonth(string storeId, string accountId, PayDebtDto payDebtDto);
}