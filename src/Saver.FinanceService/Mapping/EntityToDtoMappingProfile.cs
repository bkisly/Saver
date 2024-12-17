using AutoMapper;
using Saver.FinanceService.Contracts.Categories;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Mapping;

public class EntityToDtoMappingProfile : Profile
{
    public EntityToDtoMappingProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}