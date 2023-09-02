using System.Collections;
using System.Diagnostics.Metrics;
using AccountAuthMicroservice.Exceptions;
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

    // ===================== Method membuat New Expense =====================
    public async Task CreateNewExpense(string storeId, ExpenseCreateRequestDto requestDto)
    {
        
        // 1. Validasi apakah data Expense di bulan dan tahun yang sama ada atau tidak, Jika ada maka gagal
        var findExpense = await _context.Expenses.FirstOrDefaultAsync(e =>
            e.StoreId.Equals(storeId) && e.Date.Month.Equals(requestDto.Month) && e.Date.Year.Equals(requestDto.Year));
        if (findExpense != null)
            throw new BadRequestException("Tidak boleh ada data pengeluaran di bulan dan tahun yang sama");
        
        // 2. Setelah lolos validasi maka akan assign dto ke obejct 
        Expense saveExpense = new Expense
        {
            Id = $"EX{requestDto.Month}{requestDto.Year}-{storeId}",
            Date = new DateTime(requestDto.Year, requestDto.Month, 1),
            StoreId = storeId
        };
        saveExpense.ExpenseDetails =  requestDto.ExpenseDetails.Select((ed, Index) => new ExpenseDetail
        {
            Id = $"EXD{requestDto.Month}{requestDto.Year}{Index + 1}-{storeId}",
            Name = ed.Name,
            Description = ed.Description,
            Price = ed.Price,
            Date = new DateTime(),
            ExpenseId = saveExpense.Id,
        }).ToList();

        // 3. Save data ke database
        try
        {
            await _context.Database.BeginTransactionAsync();
            await _context.Expenses.AddAsync(saveExpense);
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    public async Task<Expense> FindExpenseById(string expenseId)
    {
        var expense = await _context.Expenses
            .Include(e => e.ExpenseDetails)
            .FirstOrDefaultAsync(e => e.Id.Equals(expenseId));
        return expense;
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
    }
}