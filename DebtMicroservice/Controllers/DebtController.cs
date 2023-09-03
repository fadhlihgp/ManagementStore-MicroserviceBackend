using DebtMicroservice.Repositories;
using DebtMicroservice.Utilities;
using DebtMicroservice.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DebtMicroservice.Controllers;

[ApiController]
[Authorize]
[Route("api/debt")]
public class DebtController : ControllerBase
{
    private IDebtRepository _debtRepository;

    public DebtController(IDebtRepository debtRepository)
    {
        _debtRepository = debtRepository;
    }

    [HttpPost, Route("new")]
    public async Task<IActionResult> CreateNewDebt([FromBody] DebtCreateDto debtCreateDto)
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        await _debtRepository.CreateNewDebt(storeId, debtCreateDto);
        return Created("api/debt/new", new
        {
            StatusCode = 201,
            Message = DataProperties.SuccessCreateDataMessage
        });
    }

    [HttpGet, Route("list")]
    public async Task<IActionResult> ListDebts()
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        var listDebts = await _debtRepository.ListDebt(storeId);
        return Ok(new
        {
            StatusCode = 200,
            Message = DataProperties.SuccessGetDataMessage,
            Data = listDebts
        });
    }

    [HttpGet, Route("{id}")]
    public async Task<IActionResult> GetDebtById([FromRoute] string id)
    {
        var debt = await _debtRepository.GetDebtById(id);
        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessGetDataMessage, Data = debt });
    }

    [HttpDelete, Route("{id}")]
    public async Task<IActionResult> DeleteDebtById([FromRoute] string id)
    {
        await _debtRepository.DeleteDebtById(id);
        return Ok(new
        {
            StatusCode = 200,
            Message = DataProperties.SuccessDeleteDataMessage
        });
    }

    [HttpPost, Route("new-next")]
    public async Task<IActionResult> AddNewDebtNext([FromBody] PayDebtDto payDebtDto)
    {
        var accountId = User.FindFirst("AccountId")?.Value;
        var storeId = User.FindFirst("StoreId")?.Value;

        await _debtRepository.AddDebtNextMonth(storeId, accountId, payDebtDto);
        return Created("api/debt/new-next",
            new { StatusCode = 201, Message = DataProperties.SuccessCreateDataMessage });
    }
}