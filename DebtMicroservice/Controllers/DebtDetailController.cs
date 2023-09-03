using DebtMicroservice.Repositories;
using DebtMicroservice.Utilities;
using DebtMicroservice.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DebtMicroservice.Controllers;

[ApiController]
[Authorize]
[Route("api/debt-detail")]
public class DebtDetailController : ControllerBase
{
    private IDebtDetailRepository _detailRepository;

    public DebtDetailController(IDebtDetailRepository detailRepository)
    {
        _detailRepository = detailRepository;
    }

    [HttpPost, Route("new")]
    public async Task<IActionResult> CreateNewDebtDetail([FromBody] DebtDetailCreateDto detailCreateDto)
    {
        await _detailRepository.CreateDebtDetail(detailCreateDto);
        return Created("api/debt-detail/new",
            new { StatusCode = 201, Message = DataProperties.SuccessCreateDataMessage });
    }

    [HttpGet, Route("{id}")]
    public async Task<IActionResult> ListDebtDetailByDebtId([FromRoute] string id)
    {
        var debtDetails = await _detailRepository.ListDebtDetailByDebtId(id);
        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessGetDataMessage, Data = debtDetails });
    }

    [HttpDelete, Route("{id}")]
    public async Task<IActionResult> DeleteDebtDetailByDebtId([FromRoute] string id)
    {
        await _detailRepository.DeleteDebtDetail(id);
        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessDeleteDataMessage });
    }

    [HttpGet, Route("detail/{id}")]
    public async Task<IActionResult> GetDebtDetailByDebtId([FromRoute] string id)
    {
        var debtDetailResponseDto = await _detailRepository.GetDebtDetailById(id);
        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessGetDataMessage, Data = debtDetailResponseDto });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDebtDetail([FromBody] DebtDetailUpdateDto updateDto)
    {
        await _detailRepository.UpdateDebtDetail(updateDto);
        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessUpdateDataMessage });
    }
}