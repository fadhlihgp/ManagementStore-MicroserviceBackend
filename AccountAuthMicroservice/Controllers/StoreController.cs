using AccountAuthMicroservice.Services;
using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;

namespace AccountAuthMicroservice.Controllers;

[ApiController]
[Route("api/account/store")]
public class StoreController : ControllerBase
{
    private IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateStore([FromBody] StoreRequestDto storeRequestDto)
    {
        var roleId = User.FindFirst("RoleId")?.Value;
        
        await _storeService.UpdateStoreAsync(storeRequestDto, roleId);
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = 200,
            Message = "Berhasil memperbarui data toko",
            Data = null
        };
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> ListStore()
    {
        var roleId = User.FindFirst("RoleId")?.Value;
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = 200,
            Message = "Berhasil mendapatkan data toko",
            Data = await _storeService.ListStoreAsync(roleId)
        };
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> FindStoreById([FromRoute] string id)
    {
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = 200,
            Message = "Berhasil mendapatkan data toko",
            Data = await _storeService.FindStoreById(id)
        };
        return Ok(result);
    }
}