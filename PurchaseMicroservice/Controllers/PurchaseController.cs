using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseMicroservice.Repositories;
using PurchaseMicroservice.Utilities;
using PurchaseMicroservice.ViewModels;

namespace PurchaseMicroservice.Controllers;

[ApiController]
[Route("api/purchase")]
[Authorize]
public class PurchaseController : ControllerBase
{
    private IPurchaseRepository _purchaseRepository;

    public PurchaseController(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    [HttpPost("create/{purchaseType}")]
    public async Task<IActionResult> CreatePurchase([FromRoute] string purchaseType, [FromBody] PurchaseRequestDto requestDto)
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        var roleId = User.FindFirst("RoleId")?.Value;

        await _purchaseRepository.CreatePurchase(storeId, roleId, purchaseType, requestDto);
        return Created("api/purchase/create", new { StatusCode = 201, Message = "Berhasil membuat transaksi" });
    }

    [HttpGet]
    public async Task<IActionResult> ListPurchase([FromQuery] int month, [FromQuery] int year)
    {
        var storeId = User.FindFirst("StoreId")?.Value;

        var purchaseResponseDtos = await _purchaseRepository.ListPurchase(storeId, month, year);
        return Ok(new
            { StatusCode = 200, Message = DataProperties.SuccessGetDataMessage, Data = purchaseResponseDtos });
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetPurchaseDetailById([FromRoute] string id)
    {
        var result = await _purchaseRepository.GetPurchaseDetail(id);
        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessGetDataMessage, Data = result });
    }
}