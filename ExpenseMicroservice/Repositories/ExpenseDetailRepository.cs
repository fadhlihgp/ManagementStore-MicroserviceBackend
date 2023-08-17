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
    public async Task CreateExpenseDetail(string storeId, ExpenseDetailRequestDto requestDto)
    {
        var findExpenseByMonthYear = await _expenseRepository.FindExpenseByMonthYear(storeId, requestDto.Month, requestDto.year);

        try
        {
            await _appDbContext.Database.BeginTransactionAsync();

            // Jika expense kosong, maka akan dibuatkan dulu expense nya, baru expense detail nya
            if (findExpenseByMonthYear == null)
            {
                Expense expense = new Expense
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = new DateTime(requestDto.year, requestDto.Month, 1),
                    StoreId = storeId
                };

                ExpenseDetail expenseDetail = new ExpenseDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = requestDto.Name,
                    Description = requestDto.Description,
                    Price = requestDto.Price,
                    Date = DateTime.Today,
                    ExpenseId = expense.Id,
                    Expense = expense
                };

                await _appDbContext.AddAsync(expenseDetail);
                await _appDbContext.Database.CommitTransactionAsync();
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                
                ExpenseDetail expenseDetail = new ExpenseDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = requestDto.Name,
                    Description = requestDto.Description,
                    Price = requestDto.Price,
                    Date = DateTime.Today,
                    ExpenseId = findExpenseByMonthYear.Id
                };

                await _appDbContext.AddAsync(expenseDetail);
                await _appDbContext.Database.CommitTransactionAsync();
                await _appDbContext.SaveChangesAsync();
            }
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