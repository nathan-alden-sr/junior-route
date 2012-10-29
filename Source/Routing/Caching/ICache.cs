using System;

namespace Junior.Route.Routing.Caching
{
	public interface ICache
	{
		void Add(string key, CacheResponse response, DateTime expirationUtcTimestamp);
		void Remove(string key);
		CacheItem Get(string key);
	}
}