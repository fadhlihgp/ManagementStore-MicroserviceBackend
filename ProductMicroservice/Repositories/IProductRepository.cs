using ProductMicroservice.ViewModels;

namespace ProductMicroservice.Repositories;

public interface IProductRepository
{
    Task CreateProduct(string storeId, string accountId, ProductRequestDto productRequestDto);
    Task UpdateProduct(string id, string storeId, string accountId, ProductRequestDto productRequestDto);
    Task DeleteProduct(string id);
    Task<IEnumerable<ProductResponseDto>> ListProducts(string storeId);
    Task AddStock(AddReduceStock addReduceStock);
    Task ReduceStock(AddReduceStock addReduceStock);
}