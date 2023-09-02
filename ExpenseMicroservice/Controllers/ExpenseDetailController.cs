using ExpenseMicroservice.Models;
using ExpenseMicroservice.Repositories;
using ExpenseMicroservice.Utilities;
using ExpenseMicroservice.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseMicroservice.Controllers;

[ApiController]
[Authorize]
[Route("api/expensedetail")]
public class ExpenseDetailController : ControllerBase
{
    private IExpenseDetailRepository _expenseDetailRepository;

    public ExpenseDetailController(IExpenseDetailRepository expenseDetailRepository)
    {
        _expenseDetailRepository = expenseDetailRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpenseDetail([FromBody] ExpenseDetailCreateDto requestDto)
    {
        await _expenseDetailRepository.CreateExpenseDetail(requestDto);
        return Created("api/expensedetail", new
        {
            StatusCode = 201,
            Message = DataProperties.SuccessCreateDataMessage
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpenseDetail([FromRoute] string id, [FromBody] ExpenseDetailUpdateRequestDto requestDto)
    {
        await _expenseDetailRepository.UpdateExpenseDetail(id, requestDto);
        return Ok(new { StatusCode = 201, Message = DataProperties.SuccessUpdateDataMessage });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpenseDetail([FromRoute] string id)
    {
        await _expenseDetailRepository.DeleteExpenseDetail(id);
        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessDeleteDataMessage });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindExpenseDetailById([FromRoute] string id)
    {
        var expenseDetail = await _expenseDetailRepository.FindExpenseDetailById(id);
        return Ok(new { StatusCode = 200, Message = DataProperties.SuccessGetDataMessage, Data = expenseDetail});
    }

    [HttpGet("list/{expenseId}")]
    public async Task<IActionResult> ListExpenseDetail([FromRoute] string expenseId)
    {
        var expenseDetailResponseDtos = await _expenseDetailRepository.ListExpenseDetail(expenseId);
        return Ok(new
            { StatusCode = 200, Message = DataProperties.SuccessGetDataMessage, Data = expenseDetailResponseDtos });
    }
}