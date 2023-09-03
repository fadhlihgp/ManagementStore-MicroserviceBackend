using DebtMicroservice.ViewModels;

namespace DebtMicroservice.Repositories;

public interface IDebtDetailRepository
{
    Task<IEnumerable<DebtDetailResponseDto>> ListDebtDetailByDebtId(string debtId);
    Task CreateDebtDetail(DebtDetailCreateDto createDto);
    Task DeleteDebtDetail(string debtDetailId);
    Task<DebtDetailResponseDto> GetDebtDetailById(string debtDetailId);
    Task UpdateDebtDetail(DebtDetailUpdateDto requestDto);
}