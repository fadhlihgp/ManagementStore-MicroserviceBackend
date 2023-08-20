using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Context;
using ProductMicroservice.Exceptions;
using ProductMicroservice.Models;
using ProductMicroservice.Utilities;
using ProductMicroservice.ViewModels;

namespace ProductMicroservice.Repositories;

public class ProductRepository : IProductRepository
{
    private AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    // ==================== Method Create Product =================
    public async Task CreateProduct(string storeId, string accountId, ProductRequestDto productRequestDto)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.ProductCode.Equals(productRequestDto.ProductCode) && p.StoreId.Equals(storeId));
        if (product != null) throw new BadRequestException("kode product tidak boleh duplikat");

        try
        {
            await _context.Database.BeginTransactionAsync();
            
            // Assign product
            var productSave = new Product
            {
                Id = Guid.NewGuid().ToString(),
                ProductCode = productRequestDto.ProductCode,
                Name = productRequestDto.Name,
                Description = productRequestDto.Description,
                Price = productRequestDto.Price,
                Stock = productRequestDto.Stock,
                CreatedAt = DateTime.Now,
                CreatedAccountId = accountId,
                EditedAt = DateTime.Now,
                EditedAccountId = accountId,
                Unit = productRequestDto.Unit,
                IsDeleted = false,
                StoreId = storeId
            };

            // Jika images dimasukkan
            if (productRequestDto.Images != null && productRequestDto.Images.Count() > 0)
            {
                productSave.Images = productRequestDto.Images.Select(p => new Image
                {
                    Id = Guid.NewGuid().ToString(),
                    ImageUrl = p.ImageUrl,
                    ProductId = productSave.Id
                });
            }

            await _context.Products.AddAsync(productSave);
            await _context.Database.CommitTransactionAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }

    // ==================== Method update product =================
    public async Task UpdateProduct(string id,string storeId, string accountId, ProductRequestDto productRequestDto)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id.Equals(id) && p.IsDeleted == false);
        // Validasi product jika null
        if (product == null) throw new NotFoundException(DataProperties.NotFoundMessage);
        
        // Validasi product jika ditemukan product lain yang produt kode nya sama di dalam satu store
        if (product.ProductCode.Equals(productRequestDto.ProductCode) && !product.Id.Equals(id) && product.StoreId.Equals(storeId))
            throw new BadRequestException("Gagal perbarui data, terdapat duplikat kode product");
        
        product.Name = productRequestDto.Name;
        product.Description = productRequestDto.Description;
        product.Stock = productRequestDto.Stock;
        product.ProductCode = productRequestDto.ProductCode;
        product.Unit = productRequestDto.Unit;
        product.EditedAt = DateTime.Now;
        product.EditedAccountId = accountId;

        try
        {
            await _context.Database.BeginTransactionAsync();

            _context.Update(product);
            
            await _context.Database.CommitTransactionAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    // ====================== Delete Product =================
    public async Task DeleteProduct(string id)
    {
        var deleteProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.Id.Equals(id) && p.IsDeleted == false);
        if (deleteProduct == null) throw new NotFoundException(DataProperties.NotFoundMessage);

        deleteProduct.IsDeleted = true;
        try
        {
            await _context.Database.BeginTransactionAsync();
            _context.Update(deleteProduct);
            await _context.Database.CommitTransactionAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    // ======================== List Product =================
    public async Task<IEnumerable<ProductResponseDto>> ListProducts(string storeId)
    {
        var products = await _context.Products
            .Include(p => p.CreatedAccount)
            .Include(p => p.EditedAccount)
            .Where(p => p.StoreId.Equals(storeId)).ToListAsync();

        IEnumerable<ProductResponseDto> result = new List<ProductResponseDto>();
        result = products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            ProductCode = p.ProductCode,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CreatedAt = p.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"),
            CreatedName = p.CreatedAccount.UserName,
            EditedAt = p.EditedAt.ToString("dd-MM-yyyy HH:mm:ss"),
            EditedName = p.EditedAccount.UserName,
            Unit = p.Unit,
            Images = p.Images == null ? null : p.Images.Select(i => new ImageResponseDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl
                })
        });

        return result;

    }

    // ========================= Add Stock =====================
    public async Task AddStock(AddReduceStock addReduceStock)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id.Equals(addReduceStock.ProductId));
        if (product == null) throw new NotFoundException(DataProperties.NotFoundMessage);
        product.Stock += addReduceStock.Stock;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    // ========================= Reduce Stock =================
    public async Task ReduceStock(AddReduceStock addReduceStock)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id.Equals(addReduceStock.ProductId));
        if (product == null) throw new NotFoundException(DataProperties.NotFoundMessage);
        if (product.Stock < addReduceStock.Stock) throw new BadRequestException("Gagal mengubah data, stok anda minus");

        try
        {
            await _context.Database.BeginTransactionAsync();
            
            product.Stock -= addReduceStock.Stock;
            _context.Products.Update(product);
            await _context.Database.CommitTransactionAsync();
            await _context.SaveChangesAsync();
            
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }
}