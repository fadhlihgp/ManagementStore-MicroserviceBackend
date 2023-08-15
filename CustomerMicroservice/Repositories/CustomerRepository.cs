using AccountAuthMicroservice.Exceptions;
using AutoMapper;
using CustomerMicroservice.Context;
using CustomerMicroservice.Models;
using CustomerMicroservice.ViewModels.Request;
using CustomerMicroservice.ViewModels.Response;
using Microsoft.EntityFrameworkCore;

namespace CustomerMicroservice.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private AppDbContext _context;
    private IMapper _mapper;

    public CustomerRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerResponseDto> FindCustomerById(string id)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id.Equals(id)
                                                                         && c.IsDeleted == false);
        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task CreateCustomer(CustomerRequestDto customerRequestDto, string storeId)
    {
        try
        {
            await _context.Database.BeginTransactionAsync();

            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = customerRequestDto.Name,
                NoHp = customerRequestDto.PhoneNumber,
                Address = customerRequestDto.Address,
                IsDeleted = false,
                StoreId = storeId
            };

            await _context.Customers.AddAsync(customer);
            await _context.Database.CommitTransactionAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    public async Task UpdateCustomer(string id, CustomerRequestDto customerRequestDto)
    {
        var customer = await FindById(id);

        try
        {
            await _context.Database.BeginTransactionAsync();

            customer.Name = customerRequestDto.Name;
            customer.Address = customerRequestDto.Address;
            customer.NoHp = customerRequestDto.PhoneNumber;
            _context.Customers.Update(customer);

            await _context.Database.CommitTransactionAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    public async Task DeleteCustomer(string id)
    {
        var customer = await FindById(id);
        
        try
        {
            await _context.Database.BeginTransactionAsync();

            customer.IsDeleted = true;
            _context.Customers.Update(customer);

            await _context.Database.CommitTransactionAsync();
            await _context.SaveChangesAsync();

        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    public async Task<List<CustomerResponseDto>> ListCustomers(string storeId)
    {
        var customers = await _context.Customers
            .Where(c => c.IsDeleted == false && c.StoreId.Equals(storeId)).ToListAsync();

        return _mapper.Map<List<CustomerResponseDto>>(customers);
    }

    public Task<List<CustomerResponseDto>> ListCustomersPagination(string storeId, int page, int size)
    {
        throw new NotImplementedException();
    }

    private async Task<Customer> FindById(string id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) throw new NotFoundException("Data customer tidak ditemukan");
        return customer;
    }
}