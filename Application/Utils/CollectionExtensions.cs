namespace Application.Utils;

public static class CollectionExtensions
{
    public static IEnumerable<T> When<T>(
        this IEnumerable<T> source,
        bool condition,
        Func<T, bool> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
}