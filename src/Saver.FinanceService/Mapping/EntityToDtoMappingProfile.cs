using AutoMapper;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Dto;

namespace Saver.FinanceService.Mapping;

public class EntityToDtoMappingProfile : Profile
{
    public EntityToDtoMappingProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}