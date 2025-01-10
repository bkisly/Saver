using Refit;

namespace Saver.FinanceService.Contracts.Categories;

public interface ICategoriesApiClient
{
    [Get("/api/finance/categories")]
    Task<ApiResponse<IEnumerable<CategoryDto>>> GetCategoriesAsync();

    [Get("/api/finance/categories/{id}")]
    Task<ApiResponse<CategoryDto>> GetCategoryByIdAsync(Guid id);

    [Post("/api/finance/categories")]
    Task<HttpResponseMessage> CreateCategoryAsync([Body] CreateCategoryRequest request);

    [Put("/api/finance/categories")]
    Task<HttpResponseMessage> EditCategoryAsync([Body] EditCategoryRequest request);

    [Delete("/api/finance/categories/{id}")]
    Task<HttpResponseMessage> DeleteCategoryAsync(Guid id);
}