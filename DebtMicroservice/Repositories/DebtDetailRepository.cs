using DebtMicroservice.Context;
using DebtMicroservice.Exceptions;
using DebtMicroservice.Models;
using DebtMicroservice.Utilities;
using DebtMicroservice.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DebtMicroservice.Repositories;

public class DebtDetailRepository : IDebtDetailRepository
{
    private AppDbContext _context;

    public DebtDetailRepository(AppDbContext context)
    {
        _context = context;
    }

    // =============================== Method ambil debt detail by debtId =============================
    public async Task<IEnumerable<DebtDetailResponseDto>> ListDebtDetailByDebtId(string debtId)
    {
        var debtDetails = await _context.DebtDetails
            .Where(d => d.DebtId.Equals(debtId))
            .Include(d => d.Product)
            .ToListAsync();

        IEnumerable<DebtDetailResponseDto> result = debtDetails.Select(dd => new DebtDetailResponseDto
        {
            Id = dd.Id,
            ProductId = dd.ProductId,
            ProductCode = dd.Product.ProductCode,
            ProductName = dd.Product.Name,
            Quantity = dd.Quantity,
            Price = dd.Price,
            Date = dd.Date
        });

        return result;
    }

    // =================== Method Create new debt detail =================
    public async Task CreateDebtDetail(DebtDetailCreateDto createDto)
    {
        // 1. Validasi apakah ada debt dengan id DebtId
        if (await _context.Debts.FindAsync(createDto.DebtId) == null)
            throw new NotFoundException(DataProperties.NotFoundMessage);
        
        // 2. Hitung jumlah data DebtDetail dengan debtId yg sama
        int count = _context.DebtDetails.Count(d => d.DebtId.Equals(createDto.DebtId));

        // 3. Inisialisasi object
        var debt = new DebtDetail
        {
            Id = $"DTL{count + 1}-{createDto.DebtId}",
            ProductId = createDto.ProductId,
            Quantity = createDto.Quantity,
            Price = createDto.Price,
            DebtId = createDto.DebtId,
            Description = createDto.Description,
            Date = DateTime.Now
        };

        // 4. Transaksi simpan
        try
        {
            await _context.Database.BeginTransactionAsync();
            await _context.DebtDetails.AddAsync(debt);
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    // ======================== Method Delete DebtDetails =================
    public async Task DeleteDebtDetail(string debtDetailId)
    {
        // 1. Validasi apakah ada debt dengan id DebtId
        var debtDetail = await _context.DebtDetails.FindAsync(debtDetailId);
        if (debtDetailId == null)
            throw new NotFoundException(DataProperties.NotFoundMessage);

        try
        {
            await _context.Database.BeginTransactionAsync();
            _context.DebtDetails.Remove(debtDetail);
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    // ============================= Method get DebtDetailById =================
    public async Task<DebtDetailResponseDto> GetDebtDetailById(string debtDetailId)
    {
        var debtDetail = await _context.DebtDetails
            .Where(d => d.Id.Equals(debtDetailId))
            .Include(d => d.Product)
            .FirstOrDefaultAsync();

        return new DebtDetailResponseDto
        {
            Id = debtDetail.Id,
            ProductId = debtDetail.ProductId,
            ProductCode = debtDetail.Product.ProductCode,
            ProductName = debtDetail.Product.Name,
            Quantity = debtDetail.Quantity,
            Price = debtDetail.Price,
            Date = debtDetail.Date
        };
    }

    // =========================== Method Update debt details =========================
    public async Task UpdateDebtDetail(DebtDetailUpdateDto requestDto)
    {
        var debtDetail = await _context.DebtDetails.FindAsync(requestDto.Id);
        if (debtDetail == null) throw new NotFoundException(DataProperties.NotFoundMessage);

        debtDetail.ProductId = requestDto.ProductId;
        debtDetail.Quantity = requestDto.Quantity;
        debtDetail.Price = requestDto.Price;

        try
        {
            await _context.Database.BeginTransactionAsync();
            _context.DebtDetails.Update(debtDetail);
            await _context.Database.CommitTransactionAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }
}