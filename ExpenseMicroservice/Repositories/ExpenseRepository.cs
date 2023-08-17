using System.Collections;
using ExpenseMicroservice.Context;
using ExpenseMicroservice.Models;
using ExpenseMicroservice.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseMicroservice.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private AppDbContext _context;

    public ExpenseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateExpense(Expense expense)
    {
        try
        {
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<ExpenseResponseDto>> ListExpenses(string storeId)
    {
        IEnumerable<Expense> expenses = await _context.Expenses
            .Include("ExpenseDetails")
            .Where(expense => expense.StoreId.Equals(storeId)).ToListAsync();

        IEnumerable<ExpenseResponseDto> result = new List<ExpenseResponseDto>();
        result = expenses.Select(expenses => new ExpenseResponseDto
        {
            Id = expenses.Id,
            Date = expenses.Date.ToString("MMMM yyyy"),
            TotalExpense = expenses.ExpenseDetails.Where(e => e.ExpenseId.Equals(expenses.Id))
                .Sum(e => e.Price)
        });
        
        return result;
    }

    public async Task<Expense> FindExpenseByMonthYear(string storeId, int month, int year)
    {
        var expense = await _context.Expenses
            .FirstOrDefaultAsync(e => e.StoreId.Equals(storeId) && e.Date.Month.Equals(month) && e.Date.Year.Equals(year));

        return expense;
        // decimal total = 0;
        // foreach (var expenseExpenseDetail in expense.ExpenseDetails)
        // {
        //     total = expense.ExpenseDetails.Where(e => e.ExpenseId.Equals(expenseExpenseDetail.Id))
        //         .Sum(e => e.Price);
        // }
        //
        // return new ExpenseResponseDto
        // {
        //     Id = expense.Id,
        //     Date = expense.Date.ToString("MMMM yyyy"),
        //     TotalExpense = total
        // };
    }
}