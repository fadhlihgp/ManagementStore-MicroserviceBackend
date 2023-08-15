using CustomerMicroservice.ViewModels.Request;
using CustomerMicroservice.ViewModels.Response;

namespace CustomerMicroservice.Repositories;

public interface ICustomerRepository
{
    Task<CustomerResponseDto> FindCustomerById(string id);
    Task CreateCustomer(CustomerRequestDto customerRequestDto, string storeId);
    Task UpdateCustomer(string id, CustomerRequestDto customerRequestDto);
    Task DeleteCustomer(string id);
    Task<List<CustomerResponseDto>> ListCustomers(string storeId);
    Task<List<CustomerResponseDto>> ListCustomersPagination(string storeId, int page, int size);
}