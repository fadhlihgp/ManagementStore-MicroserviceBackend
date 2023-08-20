using System.Collections;
using Microsoft.EntityFrameworkCore;
using PurchaseMicroservice.Context;
using PurchaseMicroservice.Exceptions;
using PurchaseMicroservice.Models;
using PurchaseMicroservice.Utilities;
using PurchaseMicroservice.ViewModels;

namespace PurchaseMicroservice.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private AppDbContext _context;
    private IProductService _productService;

    public PurchaseRepository(AppDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    public async Task CreatePurchase(string storeId, string roleId, string purchaseTypeId, PurchaseRequestDto requestDto)
    {
        if (!roleId.Equals("3")) throw new UnauthorizedException(DataProperties.UnauthorizedMessage);
        Random random = new Random();
        int randomNum = random.Next(99);
        
        var purchase = new Purchase
        {
            Id = "TR-" + purchaseTypeId + randomNum + DateTime.Now.ToString("ddMMyy"),
            Date = DateTime.Now,
            PurchaseTypeId = purchaseTypeId,
            Money = requestDto.Money,
            StoreId = storeId
        };
        
        purchase.PurchaseDetails = requestDto.PurchaseDetails.Select(p => new PurchaseDetail
        {
            Id = Guid.NewGuid().ToString(),
            PurchaseId = purchase.Id,
            ProductId = p.ProductId,
            Quantity = p.Quantity,
            Price = p.Price
        }).ToList();

        
        try
        {
            await _context.Database.BeginTransactionAsync();
            
            await _context.Purchases.AddAsync(purchase);
            foreach (var request in requestDto.PurchaseDetails)
            {
                await _productService.ReduceStock(new AddReduceRequestDto
                {
                    ProductId = request.ProductId,
                    Stock = request.Quantity
                });
            }
            
            await _context.Database.CommitTransactionAsync();
            await _context.SaveChangesAsync();
            
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<PurchaseResponseDto>> ListPurchase(string storeId, int? month, int? year)
    {
        IEnumerable<Purchase> listPurchase = new List<Purchase>();
        
        if (month.Equals(0) && !year.Equals(0))
        {
            listPurchase = await _context.Purchases
                .Where(p => p.StoreId.Equals(storeId) && p.Date.Month.Equals(month))
                .Include(p => p.PurchaseType)
                .Include(p => p.PurchaseDetails)
                .ToListAsync();
        } else if (!month.Equals(0) && year.Equals(0))
        {
            listPurchase = await _context.Purchases
                .Where(p => p.StoreId.Equals(storeId) && p.Date.Year.Equals(year))
                .Include(p => p.PurchaseType)
                .Include(p => p.PurchaseDetails)
                .ToListAsync();
        } else if (!month.Equals(0) && !year.Equals(0))
        {
            listPurchase = await _context.Purchases
                .Where(p => p.StoreId.Equals(storeId) && p.Date.Month.Equals(month) && p.Date.Year.Equals(year))
                .Include(p => p.PurchaseType)
                .Include(p => p.PurchaseDetails)
                .ToListAsync();
        }
        else
        {
            listPurchase = await _context.Purchases
                .Where(p => p.StoreId.Equals(storeId))
                .Include(p => p.PurchaseType)
                .Include(p => p.PurchaseDetails)
                .ToListAsync();
        }
        
        IEnumerable<PurchaseResponseDto> result = new List<PurchaseResponseDto>();
        result = listPurchase.Select(p => new PurchaseResponseDto
        {
            PurchaseId = p.Id,
            Date = p.Date,
            PurchaseTypeName = p.PurchaseType.Name,
            Money = p.Money,
            TotalPrice = p.PurchaseDetails
                .Sum(p => p.Price * p.Quantity)
        });
        
        return result;
    }

    public async Task<IEnumerable<PurchaseDetailResponseDto>> GetPurchaseDetail(string purchaseId)
    {
        IEnumerable<PurchaseDetail> purchaseDetails =
            await _context.PurchaseDetails.Where(p => p.PurchaseId.Equals(purchaseId))
                .Include(p => p.Product)
                .ToListAsync();

        IEnumerable<PurchaseDetailResponseDto> result = new List<PurchaseDetailResponseDto>();
        result = purchaseDetails.Select(p => new PurchaseDetailResponseDto
        {
            PurchaseDetailId = p.Id,
            ProductName = p.Product.Name,
            Quantity = p.Quantity,
            Price = p.Price,
            TotalPrice = p.Quantity * p.Price
        });
        return result;
    }
}