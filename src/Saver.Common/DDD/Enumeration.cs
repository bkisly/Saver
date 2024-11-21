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

    public static T FromName<T>(string name) where T : Enumeration
    {
        return GetAll<T>()
            .Single(x => x.Name == name);
    }

    public static bool HasName<T>(string name) where T : Enumeration
    {
        return GetAll<T>()
            .Select(x => x.Name)
            .Contains(name);
    }

    public static bool operator ==(Enumeration left, Enumeration right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Enumeration left, Enumeration right)
    {
        return !(left == right);
    }
}