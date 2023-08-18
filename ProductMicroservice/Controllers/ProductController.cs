using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.Repositories;
using ProductMicroservice.Utilities;
using ProductMicroservice.ViewModels;

namespace ProductMicroservice.Controllers;

[ApiController]
[Authorize]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRequestDto requestDto)
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        var accountId = User.FindFirst("AccountId")?.Value;

        await _productRepository.CreateProduct(storeId, accountId, requestDto);

        return Created("api/product", new { StatusCode = 201, Message = DataProperties.SuccessCreateDataMessage });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] string id, [FromBody] ProductRequestDto requestDto)
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        var accountId = User.FindFirst("AccountId")?.Value;

        await _productRepository.UpdateProduct(id, storeId, accountId, requestDto);

        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessUpdateDataMessage });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] string id)
    {
        await _productRepository.DeleteProduct(id);

        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessDeleteDataMessage });
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListProducts()
    {
        var storeId = User.FindFirst("StoreId")?.Value;

        var products = await _productRepository.ListProducts(storeId);
        
        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessGetDataMessage, Data = products });
    }

    [HttpPut("add-stock")]
    public async Task<IActionResult> AddStock([FromBody] AddReduceStock addReduceStock)
    {
        await _productRepository.AddStock(addReduceStock);
        return Ok(new { StatusCode = 200, Message = "Berhasil menambah stok produk" });
    }
    
    [HttpPut("reduce-stock")]
    public async Task<IActionResult> ReduceStock([FromBody] AddReduceStock addReduceStock)
    {
        await _productRepository.ReduceStock(addReduceStock);
        return Ok(new { StatusCode = 200, Message = "Berhasil mengurangi stok produk" });
    }
}