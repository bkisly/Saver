using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Saver.Common.DDD;

public abstract class Enumeration(int id, string name) : IComparable
{
    public int Id { get; } = id;
    [Required] public string Name { get; } = name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    public int CompareTo(object? obj) => Id.CompareTo((obj as Enumeration)?.Id);

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => Name;
}