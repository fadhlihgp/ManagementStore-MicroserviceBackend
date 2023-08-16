using AccountAuthMicroservice.Services;
using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAuthMicroservice.Controllers;

[ApiController]
[Route("/api/account")]
public class AuthController : ControllerBase
{
    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("auth/register-store")]
    public async Task<IActionResult> RegisterStore([FromBody] RegisterStoreRequestDto requestDto)
    {
        await _authService.RegisterStore(requestDto);
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = 201,
            Message = "Berhasil membuat toko baru, silahkan login menggunakan akun pemilik",
            Data = null
        };
        return Created("api/account/auth/register-store", result);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
    {
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = 200,
            Message = "Berhasil login",
            Data = await _authService.Login(requestDto)
        };
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [Route("register-member")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAccountRequestDto requestDto)
    {
        var roleId = User.FindFirst("RoleId")?.Value;
        var storeId = User.FindFirst("StoreId")?.Value;
        await _authService.RegisterAccount(requestDto, roleId, storeId);
        
        ResultResponseDto result = new ResultResponseDto
        {
            StatusCode = 201,
            Message = "Berhasil membuat akun",
            Data = null
        };
        return Created("api/account/auth/register-admin", result);
    }
}