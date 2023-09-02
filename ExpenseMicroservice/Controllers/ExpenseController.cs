using ExpenseMicroservice.Repositories;
using ExpenseMicroservice.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseMicroservice.Controllers;

[ApiController]
[Authorize]
[Route("api/expense")]
public class ExpenseController : ControllerBase
{
    private IExpenseRepository _expenseRepository;

    public ExpenseController(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ListExpense()
    {
        var storeId = User.FindFirst("StoreId")?.Value;

        return Ok(new
        {
            StatusCode = 200,
            Message = "Berhasil mendapatkan data",
            Data = await _expenseRepository.ListExpenses(storeId)
        });
    }

    [HttpPost, Route("new")]
    public async Task<IActionResult> CreateNewExpense([FromBody] ExpenseCreateRequestDto requestDto)
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        await _expenseRepository.CreateNewExpense(storeId, requestDto);
        return Created("api/expense/new", new
        {
            StatusCode = 201,
            Message = "Berhasil menyimpan data"
        });
    }
}