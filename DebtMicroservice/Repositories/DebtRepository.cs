using DebtMicroservice.Context;
using DebtMicroservice.Exceptions;
using DebtMicroservice.Models;
using DebtMicroservice.Utilities;
using DebtMicroservice.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DebtMicroservice.Repositories;

public class DebtRepository : IDebtRepository
{
    private AppDbContext _context;
    private IPurchaseRepository _purchaseRepository;
    private IDebtDetailRepository _detailRepository;

    public DebtRepository(AppDbContext context, IPurchaseRepository purchaseRepository,
        IDebtDetailRepository detailRepository)
    {
        _context = context;
        _purchaseRepository = purchaseRepository;
        _detailRepository = detailRepository;
    }

    // ========================= Method Membuat catatan hutang baru =====================
    public async Task CreateNewDebt(string storeId, DebtCreateDto createDto)
    {
        // 1. Validasi, pastikan tidak ada catatan hutang dengan customer, bulan, dan tahun yg sama dalam satu store
        var findDebt = await _context.Debts.Where(d => d.CustomerId.Equals(createDto.CustomerId) &&
                                                       d.Date.Month.Equals(createDto.Month) &&
                                                       d.Date.Year.Equals(createDto.Year) &&
                                                       d.StoreId.Equals(storeId)).FirstOrDefaultAsync();
        if (findDebt != null)
            throw new BadRequestException("Tidak boleh ada data hutang dengan customer, bulan, dan tahun yang sama");

        // 2. Pembuatan Id Debt 
        int totalDebt = _context.Debts
            .Count(d => d.StoreId.Equals(storeId) && 
                        d.Date.Month.Equals(createDto.Month) && 
                        d.Date.Year.Equals(createDto.Year));
        
        string id = $"DT{createDto.Month}{createDto.Year}{totalDebt + 1}-{storeId}";
        
        // 3. Assign dari dto ke object Debt
        Debt debt = new Debt
        {
            Id = id,
            Date = new DateTime(createDto.Year, createDto.Month, 1),
            CustomerId = createDto.CustomerId,
            IsPaid = false,
            StoreId = storeId,
            Money = 0,
            DebtDetails = createDto.DebtDetails.Select((p, Index) => new DebtDetail
            {
                Id = $"DTL{createDto.Month}{createDto.Year}{Index + 1}-{storeId}",
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                Price = p.Price,
                DebtId = id,
                Date = DateTime.Now
            }).ToList()
        };

        // 4. Lakukan Transaksi penyimpanan
        try
        {
            await _context.Database.BeginTransactionAsync();
            await _context.AddAsync(debt);
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    // ===================== Method List Debt =================
    public async Task<IEnumerable<DebtResponseDto>> ListDebt(string storeId)
    {
        // 1. Ambil data debt
        var debts = await _context.Debts
            .Where(d => d.StoreId.Equals(storeId))
            .Include(d => d.DebtDetails)
            .Include(d => d.Customer)
            .Include("DebtDetails.Product")
            .ToListAsync();
        
        // 2. Assign ke response Dto
        IEnumerable<DebtResponseDto> debtResponses = debts.Select(d => new DebtResponseDto
        {
            DebtId = d.Id,
            Date = d.Date,
            CustomerId = d.CustomerId,
            CustomerName = d.Customer.Name,
            IsPaid = d.IsPaid,
            Total = d.DebtDetails.Sum(d => d.Price * d.Quantity),
            DebtDetails = d.DebtDetails.Select(dd => new DebtDetailResponseDto
            {
                Id = dd.Id,
                ProductId = dd.ProductId,
                ProductCode = dd.Product.ProductCode,
                ProductName = dd.Product.Name,
                Quantity = dd.Quantity,
                Price = dd.Price,
                Date = dd.Date
            })
        }).ToList();

        return debtResponses;
    }

    // ==================== Method debt by Id =================
    public async Task<DebtResponseDto> GetDebtById(string debtId)
    {
        // 1. Ambil data 
        var debt = await _context.Debts
            .Include(d => d.DebtDetails)
            .Include(d => d.Customer)
            .Include("DebtDetails.Product")
            .FirstOrDefaultAsync(d => d.Id.Equals(debtId));

        if (debt == null) throw new NotFoundException(DataProperties.NotFoundMessage);
        DebtResponseDto debtResponse = new DebtResponseDto
        {
            DebtId = debt.Id,
            Date = debt.Date,
            CustomerId = debt.CustomerId,
            CustomerName = debt.Customer.Name,
            IsPaid = debt.IsPaid,
            Total = !debt.DebtDetails.IsNullOrEmpty() ? debt.DebtDetails.Sum(d => d.Price * d.Quantity) : 0,
            DebtDetails = debt.DebtDetails.Select(dd => new DebtDetailResponseDto
            {
                Id = dd.Id,
                ProductId = dd.ProductId,
                ProductCode = dd.Product.ProductCode,
                ProductName = dd.Product.Name,
                Quantity = dd.Quantity,
                Price = dd.Price,
                Date = dd.Date
            }).ToList()
        };

        return debtResponse;
    }

    // ================= Method delete debt =====================
    public async Task DeleteDebtById(string debtId)
    {
        var findDebt = await _context.Debts.Where(d => d.Id.Equals(debtId))
            .Include(d => d.DebtDetails).FirstOrDefaultAsync();

        if (findDebt == null) throw new NotFoundException(DataProperties.NotFoundMessage);
        if (findDebt.IsPaid) throw new BadRequestException("Tidak dapan menghapus data hutang yang sudah dibayar");
        
        try
        {
            await _context.Database.BeginTransactionAsync();
            _context.RemoveRange(findDebt.DebtDetails);
            _context.Remove(findDebt);
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {

            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    // ================== Method membayar hutang ================
    public async Task PayDebt(PayDebtDto payDto)
    {
        // 1. Mendapatkan data debt dulu beserta dengan debt details
        var debt = await _context.Debts
            .Include(d => d.DebtDetails)
            .Include(d => d.Customer)
            .Include("DebtDetails.Product")
            .FirstOrDefaultAsync(d => d.Id.Equals(payDto.DebtId));
        
        // 2. Validasi
        if (debt == null) throw new NotFoundException(DataProperties.NotFoundMessage);
        if (debt.IsPaid) throw new BadRequestException("Transaksi gagal, Data hutang sudah terbayar sebelumnya");

        // 3. Assign ke request Dto
        PurchaseRequestDto requestDto = new PurchaseRequestDto
        {
            Money = payDto.Money,
            PurchaseDetails = debt.DebtDetails.Select(dd => new PurchaseDetailRequestDto
            {
                ProductId = dd.ProductId,
                Quantity = dd.Quantity,
                Price = dd.Price
            })
        };
        
        // 4. Transaksi
        try
        {
            await _context.Database.BeginTransactionAsync();
            debt.IsPaid = true;
            _context.Debts.Update(debt);
            await _purchaseRepository.CreateNewPurchase(requestDto);
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
        
    }

    // ==================== Method menambahkan otomatis kekurangan bayar hutang di bulan selanjutnya =================
    public async Task AddDebtNextMonth(string storeId, string accountId, PayDebtDto payDebtDto)
    {
        // 1. Ambil Debt berdasarkan Id, (merupakan debt yg telah dibayar)
        var debtResponseDto = await GetDebtById(payDebtDto.DebtId);
        
        // 1a. Antisipasi jika ternyata bulan berada di Desember maka next month nya adalah tahun baru
        int month = 0;
        int year = 0;
        if (debtResponseDto.Date.Month.Equals(12))
        {
            month = 1;
            year = debtResponseDto.Date.Year + 1;
        }
        else
        {
            month = debtResponseDto.Date.Month + 1;
            year = debtResponseDto.Date.Year;
        }
        
        // 2. Filter debt apakah ada data hutang di bulan depan dengan customer yang sama
        var debtFilter = await _context.Debts.Where(d => d.CustomerId.Equals(debtResponseDto.CustomerId) 
                                                         && d.Date.Month.Equals(month) &&
                                                         d.Date.Year.Equals(year) &&
                                                         d.StoreId.Equals(storeId)).FirstOrDefaultAsync();
        
        // 3. Mencari product, karena ketika otomatis menambah data hutang, butuh suatu produk yg pada case ini
        // di inisialisasi sebagai product "Lainya"
        var product = await _context.Products.Where(p => p.StoreId.Equals(storeId) && p.Name.Equals("Lainya"))
            .FirstOrDefaultAsync();

        Product? saveProduct = null;

        try
        { 
            // 4. Validasi product jika null, Maka akan membuat product "Lainya" didalam store tersebut
            if (product == null)
            {
                saveProduct = new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductCode = "000",
                    Name = "Lainya",
                    Description = null,
                    Price = 0,
                    Stock = 99999,
                    CreatedAt = DateTime.Now,
                    CreatedAccountId = accountId,
                    EditedAt = DateTime.Now,
                    EditedAccountId = accountId,
                    Unit = "Pcs",
                    IsDeleted = false,
                    StoreId = storeId
                };

                await _context.Products.AddAsync(saveProduct);
                await _context.SaveChangesAsync();
            }
            
            // 5. Jika terdapat hutang di bulan depan dengan customer yang sama, maka debtDetail akan masuk ke data tersebut
            if (debtFilter != null)
            {
                DebtDetailCreateDto debtDetailCreateDto = new DebtDetailCreateDto
                {
                    DebtId = debtFilter.Id,
                    ProductId = product.Id.IsNullOrEmpty() ? saveProduct.Id : product.Id,
                    Quantity = 1,
                    Description = $"Kekurangan hutang bulan{debtFilter.Date.ToString("MMMM yyyy")}",
                    Price = payDebtDto.Money
                };

                await _detailRepository.CreateDebtDetail(debtDetailCreateDto);
            }
            
            // 6. Jika ternyata filter debt dibulan depan tidak ada, maka akan membuat Data Debt terlebih dahulu, baru membuat DebtDetails nya
            else
            {
                DebtCreateDto debtCreateDto = new DebtCreateDto
                {
                    CustomerId = debtResponseDto.CustomerId,
                    Month = month,
                    Year = year
                };
                debtCreateDto.DebtDetails = new List<DebtDetailRequestDto>
                {
                    new()
                    {
                        ProductId = product.Id.IsNullOrEmpty() ? saveProduct.Id : product.Id,
                        Quantity = 1,
                        Price = payDebtDto.Money
                    }
                };
                await CreateNewDebt(storeId, debtCreateDto);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}