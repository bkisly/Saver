namespace Saver.FinanceService.Api;

public static class CategoriesApi
{
    public static IEndpointRouteBuilder MapCategoriesApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/categories");

        api.MapGet("/", GetCategories);
        api.MapGet("/{id:int}", GetCategoryById);
        api.MapPost("/", CreateCategory);
        api.MapPut("/{id:int}", EditCategory);
        api.MapDelete("/{id:int}", DeleteCategory);

        return builder;
    }

    private static void GetCategories()
    {

    }

    private static void GetCategoryById()
    {

    }

    private static void CreateCategory()
    {

    }

    private static void EditCategory()
    {

    }

    private static void DeleteCategory()
    {

    }
}