using System;
using System.Threading.Tasks;

using Junior.Common;

namespace Junior.Route.Routing.Caching
{
	public class NoCache : ICache
	{
		public Task AddAsync(string key, CacheResponse response, DateTime expirationUtcTimestamp)
		{
			return Task.Factory.Empty();
		}

		public Task RemoveAsync(string key)
		{
			return Task.Factory.Empty();
		}

		public Task<CacheItem> GetAsync(string key)
		{
			return Task<CacheItem>.Factory.Empty();
		}
	}
}