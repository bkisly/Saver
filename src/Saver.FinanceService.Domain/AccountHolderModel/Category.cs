using CSharpFunctionalExtensions;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class Category : Entity<Guid>
{
    public override Guid Id { get; protected set; } = Guid.NewGuid();
    public string Name { get; internal set; } = null!;
    public string? Description { get; set; }

    private Category()
    { }

    public Category(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}