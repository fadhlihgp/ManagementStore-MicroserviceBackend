using ExpenseMicroservice.ViewModels;

namespace ExpenseMicroservice.Repositories;

public interface IExpenseDetailRepository
{
    Task CreateExpenseDetail(string storeId, ExpenseDetailRequestDto requestDto);
    Task UpdateExpenseDetail(string id, ExpenseDetailUpdateRequestDto requestDto);
    Task DeleteExpenseDetail(string expenseDetailId);
    Task<IEnumerable<ExpenseDetailResponseDto>> ListExpenseDetail(string expenseId);
    Task<ExpenseDetailResponseDto> FindExpenseDetailById(string id);
}