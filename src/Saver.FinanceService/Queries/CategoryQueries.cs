using AutoMapper;
using Saver.FinanceService.Contracts.Categories;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Queries;

public class CategoryQueries(IAccountHolderService accountHolderService, IMapper mapper) : ICategoryQueries
{
    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        return accountHolder?.Categories.Select(mapper.Map<Category, CategoryDto>) ?? [];
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        var category = accountHolder?.Categories.SingleOrDefault(x => x.Id == id);
        return category is not null ? mapper.Map<Category, CategoryDto>(category) : null;
    }
}