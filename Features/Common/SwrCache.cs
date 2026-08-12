using System.Collections.Concurrent;

namespace LoreWeaver.Features.Common;

public class SwrCache<T>
{
    private readonly ConcurrentDictionary<string, Entry> _store = new();

    public (bool Found, T? Value, bool Stale) TryGet(string key)
    {
        if (!_store.TryGetValue(key, out var entry)) return (false, default, false);

        var now = DateTimeOffset.UtcNow;
        if (now > entry.StaleAt)
        {
            _store.TryRemove(key, out _);
            return (false, default, false);
        }

        return (true, entry.Value, now > entry.ExpiresAt);
    }

    public void Set(string key, T value, TimeSpan ttl, TimeSpan staleWindow)
    {
        var now = DateTimeOffset.UtcNow;
        _store[key] = new Entry(value, now + ttl, now + ttl + staleWindow);
    }

    private record Entry(T Value, DateTimeOffset ExpiresAt, DateTimeOffset StaleAt);
}
