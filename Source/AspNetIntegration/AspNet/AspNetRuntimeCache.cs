using System;
using System.Threading.Tasks;
using System.Web.Caching;

using Junior.Common;
using Junior.Route.Routing;
using Junior.Route.Routing.Caching;

namespace Junior.Route.AspNetIntegration.AspNet
{
	public class AspNetRuntimeCache : ICache
	{
		private readonly IHttpRuntime _httpRuntime;
		private readonly ISystemClock _systemClock;

		public AspNetRuntimeCache(IHttpRuntime httpRuntime, ISystemClock systemClock)
		{
			systemClock.ThrowIfNull("systemClock");
			systemClock.ThrowIfNull("systemClock");

			_httpRuntime = httpRuntime;
			_systemClock = systemClock;
		}

		public Task AddAsync(string key, CacheResponse response, DateTime expirationUtcTimestamp)
		{
			key.ThrowIfNull("key");
			response.ThrowIfNull("response");

			if (expirationUtcTimestamp.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Expiration must be UTC.", "expirationUtcTimestamp");
			}

			var cacheItem = new CacheItem(response, _systemClock.UtcDateTime);

			_httpRuntime.Cache.Insert(key, cacheItem, null, expirationUtcTimestamp, Cache.NoSlidingExpiration);

			return Task.Factory.Empty();
		}

		public Task RemoveAsync(string key)
		{
			key.ThrowIfNull("key");

			_httpRuntime.Cache.Remove(key);

			return Task.Factory.Empty();
		}

		public Task<CacheItem> GetAsync(string key)
		{
			key.ThrowIfNull("key");

			return ((CacheItem)_httpRuntime.Cache.Get(key)).AsCompletedTask();
		}
	}
}