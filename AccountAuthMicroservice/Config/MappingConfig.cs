using AccountAuthMicroservice.Entities;
using AccountAuthMicroservice.ViewModels;
using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;
using AutoMapper;

namespace AccountAuthMicroservice.Config;

public class MappingConfig
{
    // Konfigurasi dan register Mapper
    public static MapperConfiguration RegisterMapper()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<Store, StoreResponseDto>();
            config.CreateMap<StoreResponseDto, Store>();
            config.CreateMap<Store, StoreRequestDto>();
            config.CreateMap<StoreRequestDto, Store>();
        });
        return mappingConfig;
    } 
}