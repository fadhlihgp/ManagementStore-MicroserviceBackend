using System.Net;
using System.Net.Http.Headers;
using System.Text;
using DebtMicroservice.Exceptions;
using DebtMicroservice.ViewModels;
using Newtonsoft.Json;

namespace DebtMicroservice.Utilities;

public class BaseService : IBaseService
{
    private IHttpClientFactory _httpClientFactory;
    private IHttpContextAccessor _httpContextAccessor;

    public BaseService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResponseDto>? SendAsync(RequestDto requestDto)
    {
        using var apiClient = _httpClientFactory.CreateClient("ManagementStore");
        
        // Menambahkan header Authorization dengan nilai token bearer
        var token =
            GetAuthorizationToken();
        
        // Memasang token di Product
        apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var apiUrl = requestDto.Url;
        
        var requestContent = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");

        HttpResponseMessage? apiResponse = new HttpResponseMessage();
        
        switch (requestDto.ApiType)
        {
            case ApiType.PUT :
                apiResponse = await apiClient.PutAsync(apiUrl, requestContent);
                break;
            case ApiType.DELETE :
                apiResponse = await apiClient.DeleteAsync(apiUrl);
                break;
            case ApiType.POST :
                apiResponse = await apiClient.PostAsync(apiUrl, requestContent);
                break;
            default:
                apiResponse = await apiClient.GetAsync(apiUrl);
                break;
        }
        
        //using var apiResponse = await apiClient.PutAsync(apiUrl, requestContent);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

        switch (apiResponse.StatusCode)
        {
            case HttpStatusCode.BadRequest :
                throw new BadRequestException(apiResponseDto.Message);
            case HttpStatusCode.NotFound :
                throw new NotFoundException(apiResponseDto.Message);
            case HttpStatusCode.Unauthorized :
                throw new UnauthorizedException(apiResponseDto.Message);
            case HttpStatusCode.InternalServerError :
                throw new Exception(apiResponseDto.Message);
        }

        return apiResponseDto;
    }
    
    private string GetAuthorizationToken()
    {
        string authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"];
        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            return authorizationHeader.Substring("Bearer ".Length);
        }
        return null;
    }
}