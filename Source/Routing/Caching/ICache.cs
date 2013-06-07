using System;
using System.Threading.Tasks;

namespace Junior.Route.Routing.Caching
{
	public interface ICache
	{
		Task AddAsync(string key, CacheResponse response, DateTime expirationUtcTimestamp);
		Task RemoveAsync(string key);
		Task<CacheItem> GetAsync(string key);
	}
}