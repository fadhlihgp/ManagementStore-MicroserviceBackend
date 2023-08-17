using ExpenseMicroservice.Repositories;
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
}