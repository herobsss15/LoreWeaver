namespace LoreWeaver.Features.Common;

public class SeededRandom
{
    private readonly Random _random;

    public SeededRandom(string seed) => _random = new Random(Fnv1aHash(seed));

    public T Pick<T>(IReadOnlyList<T> list) => list[_random.Next(list.Count)];

    public List<T> PickMany<T>(IReadOnlyList<T> list, int count)
    {
        var pool = new List<T>(list);
        var results = new List<T>();
        for (var i = 0; i < count && pool.Count > 0; i++)
        {
            var index = _random.Next(pool.Count);
            results.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return results;
    }

    private static int Fnv1aHash(string value)
    {
        unchecked
        {
            const uint offsetBasis = 2166136261;
            const uint prime = 16777619;
            var hash = offsetBasis;
            foreach (var ch in value)
            {
                hash ^= ch;
                hash *= prime;
            }

            return (int)hash;
        }
    }
}
