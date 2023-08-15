using CustomerMicroservice.Repositories;
using CustomerMicroservice.ViewModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerMicroservice.Controllers;

[Authorize]
[ApiController]
[Route("api/customer")]
public class CustomerController : ControllerBase
{
    private ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpPost]
    public async Task<ActionResult> CreateCustomer([FromBody] CustomerRequestDto customerRequestDto)
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        await _customerRepository.CreateCustomer(customerRequestDto, storeId);

        return Created("api/customer/create",new
        {
            StatusCode = 201,
            Message = "Berhasil membuat data customer"
        });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromRoute] String id, [FromBody] CustomerRequestDto customerRequestDto)
    {
        await _customerRepository.UpdateCustomer(id, customerRequestDto);

        return Ok(new
        {
            StatusCode = 200,
            Message = "Berhasil memperbarui data customer"
        });
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<IActionResult> DeleteCustomer([FromRoute] String id)
    {
        await _customerRepository.DeleteCustomer(id);

        return Ok(new
        {
            StatusCode = 200,
            Message = "Berhasil menghapus data customer"
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> FindCustomerById([FromRoute] String id)
    {
        var findCustomerById = await _customerRepository.FindCustomerById(id);

        return Ok(new
        {
            StatusCode = 200,
            Message = "Berhasil menghapus data customer",
            Data = findCustomerById
        });
    }

    [HttpGet]
    public async Task<IActionResult> ListCustomer()
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        var customers = await _customerRepository.ListCustomers(storeId);

        return Ok(new
        {
            StatusCode = 201,
            Message = "Berhasil mendapatkan data customer",
            Data = customers
        });
    }
}