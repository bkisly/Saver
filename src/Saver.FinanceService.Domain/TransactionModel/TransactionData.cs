using CSharpFunctionalExtensions;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Domain.TransactionModel;

public class TransactionData : ValueObject, ICloneable
{
    public string Name { get; } = null!;
    public string? Description { get; }
    public decimal Value { get; }
    public Currency Currency { get; } = null!;
    public Category? Category { get; }

    private TransactionData()
    { }

    public TransactionData(string name, string? description, decimal value, Currency currency, Category? category)
    {
        if (string.IsNullOrEmpty(name))
            throw new FinanceDomainException("Transaction name cannot be empty.", FinanceDomainErrorCode.EmptyValue);

        if (value == 0)
            throw new FinanceDomainException("Transaction value cannot be 0.", FinanceDomainErrorCode.InvalidValue);

        Name = name;
        Description = description;
        Value = value;
        Category = category;
        Currency = currency;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;

        if (Description != null)
            yield return Description;

        yield return Value;
        yield return Currency;

        if (Category != null)
            yield return Category;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}