using AutoMapper;

namespace DebtMicroservice.Config;

public class MappingConfig
{
    // Konfigurasi dan register Mapper
    public static MapperConfiguration RegisterMapper()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            // config.CreateMap<ExpenseDetail, ExpenseDetailResponseDto>();
            // config.CreateMap<ExpenseDetailResponseDto, ExpenseDetail>();
        });
        return mappingConfig;
    } 
}