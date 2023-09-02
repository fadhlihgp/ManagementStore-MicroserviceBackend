using AccountAuthMicroservice.Exceptions;
using ExpenseMicroservice.Context;
using ExpenseMicroservice.Models;
using ExpenseMicroservice.Utilities;
using ExpenseMicroservice.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseMicroservice.Repositories;

public class ExpenseDetailRepository : IExpenseDetailRepository
{
    private AppDbContext _appDbContext;
    private IExpenseRepository _expenseRepository;

    public ExpenseDetailRepository(AppDbContext appDbContext, IExpenseRepository expenseRepository)
    {
        _appDbContext = appDbContext;
        _expenseRepository = expenseRepository;
    }

    // ======================= Method untuk membuat expense detail =======================
    public async Task CreateExpenseDetail(ExpenseDetailCreateDto requestDto)
    {
        var findExpenseById = await _expenseRepository.FindExpenseById(requestDto.ExpenseId);
        if (findExpenseById == null)
            throw new NotFoundException("Data pengeluaran tidak ditemukan");

        int count = findExpenseById.ExpenseDetails.Count();
        try
        {
            await _appDbContext.Database.BeginTransactionAsync();

            ExpenseDetail expenseDetail = new ExpenseDetail
            {
                Id = $"EXD{findExpenseById.Date.Month}{findExpenseById.Date.Year}{count + 1}-{findExpenseById.StoreId}",
                Name = requestDto.Name,
                Description = requestDto.Description,
                Price = requestDto.Price,
                Date = new DateTime(),
                ExpenseId = requestDto.ExpenseId
            };

            await _appDbContext.AddAsync(expenseDetail);
            await _appDbContext.SaveChangesAsync();
            await _appDbContext.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _appDbContext.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
        
    }

    // ========================== Method Update Expense Details =========================
    public async Task UpdateExpenseDetail(string id, ExpenseDetailUpdateRequestDto requestDto)
    {
        var findExpenseDetail = await _appDbContext.ExpenseDetails
            .FirstOrDefaultAsync(e => e.Id.Equals(id));

        if (findExpenseDetail == null) throw new NotFoundException(DataProperties.NotFoundMessage);
        try
        {
            await _appDbContext.Database.BeginTransactionAsync();

            findExpenseDetail.Name = requestDto.Name;
            findExpenseDetail.Price = requestDto.Price;
            findExpenseDetail.Description = requestDto.Description;
            _appDbContext.Update(findExpenseDetail);
            await _appDbContext.Database.CommitTransactionAsync();
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _appDbContext.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    // ========================= Method Delete ExpenseDetail =========================
    public async Task DeleteExpenseDetail(string expenseDetailId)
    {
        var findExpenseDetail = await _appDbContext.ExpenseDetails
            .FirstOrDefaultAsync(e => e.Id.Equals(expenseDetailId));
        if (findExpenseDetail == null) throw new NotFoundException(DataProperties.NotFoundMessage);

        _appDbContext.ExpenseDetails.Remove(findExpenseDetail);
        await _appDbContext.SaveChangesAsync();
    }

    // ============================= Method List Expense Detail by ExpenseId ==========================
    public async Task<IEnumerable<ExpenseDetailResponseDto>> ListExpenseDetail(string expenseId)
    {
        IEnumerable<ExpenseDetail> listExpenseDetails =
            await _appDbContext.ExpenseDetails.Where(e => e.ExpenseId.Equals(expenseId)).ToListAsync();
        
        IEnumerable<ExpenseDetailResponseDto> result = new List<ExpenseDetailResponseDto>();
        result = listExpenseDetails.Select(e => new ExpenseDetailResponseDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Date = e.Date.ToString("dd/MM/yyyy"),
            Price = e.Price
        });
        return result;
    }

    public async Task<ExpenseDetailResponseDto> FindExpenseDetailById(string id)
    {
        var expenseDetail = await _appDbContext.ExpenseDetails
            .FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (expenseDetail == null) throw new NotFoundException(DataProperties.NotFoundMessage);

        ExpenseDetailResponseDto result = new ExpenseDetailResponseDto
        {
            Id = expenseDetail.Id,
            Name = expenseDetail.Name,
            Description = expenseDetail.Description,
            Date = expenseDetail.Date.ToString("dd/MM/yyyy"),
            Price = expenseDetail.Price
        };
        return result;
    }
}