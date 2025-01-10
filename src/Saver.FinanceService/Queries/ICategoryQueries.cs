using Saver.FinanceService.Contracts.Categories;

namespace Saver.FinanceService.Queries;

public interface ICategoryQueries
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
}