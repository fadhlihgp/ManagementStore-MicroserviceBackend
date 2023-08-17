using ExpenseMicroservice.Models;
using ExpenseMicroservice.ViewModels;

namespace ExpenseMicroservice.Repositories;

public interface IExpenseRepository
{
    Task CreateExpense(Expense expense);
    Task<IEnumerable<ExpenseResponseDto>> ListExpenses(string storeId);
    Task<Expense> FindExpenseByMonthYear(string storeId, int month, int year);
}