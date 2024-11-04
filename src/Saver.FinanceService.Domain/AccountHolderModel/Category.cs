using CSharpFunctionalExtensions;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class Category : Entity<Guid>
{
    public string Name { get; internal set; }
    public string? Description { get; set; }

    private Category()
    { }

    public Category(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}