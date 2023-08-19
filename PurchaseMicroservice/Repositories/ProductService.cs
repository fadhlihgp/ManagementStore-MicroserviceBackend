using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System;
using Newtonsoft.Json;
using PurchaseMicroservice.Exceptions;
using PurchaseMicroservice.Utilities;
using PurchaseMicroservice.ViewModels;

namespace PurchaseMicroservice.Repositories;

public class ProductService : IProductService
{
    private IHttpClientFactory _httpClientFactory;
    private IHttpContextAccessor _httpContextAccessor;

    public ProductService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = contextAccessor;
    }

    public async Task<ResponseDto> ReduceStock(AddReduceRequestDto requestDto)
    {
        using var apiClient = _httpClientFactory.CreateClient("Product");
        
        // Menambahkan header Authorization dengan nilai token bearer
        var token =
            GetAuthorizationToken();
        
        // Memasang token di Product
        apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var apiUrl = $"{ApiUrl.ProductUrl}/api/product/reduce-stock";
        
        var requestContent = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

        using var apiResponse = await apiClient.PutAsync(apiUrl, requestContent);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

        if (apiResponse.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new BadRequestException(apiResponseDto.Message);
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