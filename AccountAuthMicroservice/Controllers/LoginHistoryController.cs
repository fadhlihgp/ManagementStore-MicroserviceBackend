using AccountAuthMicroservice.Services;
using AccountAuthMicroservice.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAuthMicroservice.Controllers;

[ApiController]
[Authorize]
[Route("api/account/loginhistory")]
public class LoginHistoryController : ControllerBase
{
    private ILoginHistoryService _loginHistoryService;

    public LoginHistoryController(ILoginHistoryService loginHistoryService)
    {
        _loginHistoryService = loginHistoryService;
    }

    [HttpGet]
    [Route("list-store")]
    public async Task<IActionResult> ListLoginHistoryByStoreId()
    {
        var storeId = User.FindFirst("StoreId")?.Value;
        var roleId = User.FindFirst("RoleId")?.Value;

        var listLoginHistoryByStoreId = await _loginHistoryService.ListLoginHistoryByStoreId(storeId, roleId);
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = 200,
            Message = "Berhasil mendapatkan data",
            Data = listLoginHistoryByStoreId
        };
        return Ok(result);
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> ListLogniHistory()
    {
        var roleId = User.FindFirst("RoleId")?.Value;

        var listLogin = await _loginHistoryService.ListLoginHistory(roleId);
        return Ok( new ResultResponseDto
        {
            StatusCode = 200,
            Message = "Berhasil mendapatkan data",
            Data = listLogin
        });
    }
}