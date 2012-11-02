using System;

namespace Junior.Route.Routing.Caching
{
	public class NoCache : ICache
	{
		public void Add(string key, CacheResponse response, DateTime expirationUtcTimestamp)
		{
		}

		public void Remove(string key)
		{
		}

		public CacheItem Get(string key)
		{
			return null;
		}
	}
}