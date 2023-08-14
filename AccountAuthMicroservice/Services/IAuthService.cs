using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;

namespace AccountAuthMicroservice.Services;

public interface IAuthService
{
    Task RegisterStore(RegisterStoreRequestDto storeRequestDto);
    Task RegisterAccount(RegisterAccountRequestDto accountRequestDto, string role, string storeId);
    Task<LoginResponseDto> Login (LoginRequestDto requestDto);
}