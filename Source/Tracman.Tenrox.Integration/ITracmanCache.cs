using System;

namespace Tracman.Tenrox.Integration
{
    public interface ITracmanCache
    {
        T Get<T>(string key);
        T TryGetSet<T>(string key, Func<T> actionToLoadData);
    }
}