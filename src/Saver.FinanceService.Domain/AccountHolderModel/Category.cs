using CSharpFunctionalExtensions;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class Category : Entity<Guid>
{
    public override Guid Id { get; protected set; } = Guid.NewGuid();
    public string Name { get; internal set; } = null!;
    public string? Description { get; set; }

    public Guid AccountHolderId { get; protected set; }

    private Category()
    { }

    public Category(string name, string? description, Guid accountHolderId)
    {
        Name = name;
        Description = description;
        AccountHolderId = accountHolderId;
    }
}