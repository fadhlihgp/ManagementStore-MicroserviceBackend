using AccountAuthMicroservice.Services;
using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAuthMicroservice.Controllers;

[ApiController]
[Route("/api/account/auth")]
public class AuthController : ControllerBase
{
    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("register-store")]
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
}