using AutoMapper;
using CustomerMicroservice.Models;
using CustomerMicroservice.ViewModels.Response;

namespace CustomerMicroservice.Config;

public class MappingConfig
{
    // Konfigurasi dan register Mapper
    public static MapperConfiguration RegisterMapper()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<Customer, CustomerResponseDto>();
            config.CreateMap<CustomerResponseDto, Customer>();
        });
        return mappingConfig;
    } 
}