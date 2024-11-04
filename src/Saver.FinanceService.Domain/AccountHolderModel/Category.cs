using CSharpFunctionalExtensions;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class Category : Entity<Guid>
{
    private string _name = null!;
    public string Name
    {
        get => _name;
        internal set
        {
            if (string.IsNullOrEmpty(value))
                throw new FinanceDomainException("Name of the category cannot be empty.", 
                    FinanceDomainErrorCode.EmptyValue);

            _name = value;
        }
    }

    public string? Description { get; set; }

    private Category()
    { }

    public Category(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}