using System.Net;
using AccountAuthMicroservice.Config;
using AccountAuthMicroservice.Services;
using AccountAuthMicroservice.ViewModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAuthMicroservice.Controllers;

[ApiController]
[Authorize]
[Route("api/account/manage")]
public class AccountController : ControllerBase
{
    private IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListAccount()
    {
        var roleId = User.FindFirst("RoleId")?.Value;

        var listAccount = await _accountService.ListAccount(roleId);
        return Ok(new
        {
            StatusCode = 200,
            Message = "Berhasil mendapatkan data akun",
            Data = listAccount
        });
    }

    [HttpGet("list-owner")]
    public async Task<IActionResult> ListAccountBaseStoreId()
    {
        var roleId = User.FindFirst("RoleId")?.Value;
        var storeId = User.FindFirst("StoreId")?.Value;

        var listAccountBaseStoreIds = await _accountService.ListAccountBaseStoreIds(roleId, storeId);
        return Ok(new
        {
            StatusCode = 200,
            Message = "Berhasil mendapatkan data akun",
            Data = listAccountBaseStoreIds
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody]AccountRequestDto accountRequestDto)
    {
        var roleId = User.FindFirst("RoleId")?.Value;

        await _accountService.CreateAccount(roleId, accountRequestDto);
        return Created("api/account/manage",new
        {
            StatusCode = 201,
            Message = "Berhasil membuat data akun"
        });
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAccount([FromRoute] string id,
        [FromBody] AccountRequestDto accountRequestDto)
    {
        await _accountService.UpdateAccount(id, accountRequestDto);
        return Created("api/account/manage",new
        {
            StatusCode = 200,
            Message = "Berhasil memperbarui data akun"
        });
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] string id)
    {
        await _accountService.DeleteAccount(id);
        return Ok(new
        {
            StatusCode = 200,
            Message = "Berhasil menghapus data akun"
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetAccountById([FromRoute] string id)
    {
        return Ok(new
        {
            StatusCode = HttpStatusCode.OK,
            Message = "Berhasil mendapatkan data akun",
            Data = await _accountService.AccountById(id)
        });
    }
}