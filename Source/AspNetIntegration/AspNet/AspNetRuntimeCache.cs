using System;
using System.Web;
using System.Web.Caching;

using Junior.Common;
using Junior.Route.Routing.Caching;

namespace Junior.Route.AspNetIntegration.AspNet
{
	public class AspNetRuntimeCache : ICache
	{
		private readonly ISystemClock _systemClock;

		public AspNetRuntimeCache(ISystemClock systemClock)
		{
			systemClock.ThrowIfNull("systemClock");

			_systemClock = systemClock;
		}

		public void Add(string key, CacheResponse response, DateTime expirationUtcTimestamp)
		{
			key.ThrowIfNull("key");
			response.ThrowIfNull("response");

			if (expirationUtcTimestamp.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Expiration must be UTC.", "expirationUtcTimestamp");
			}

			var cacheItem = new CacheItem(response, _systemClock.UtcDateTime);

			HttpRuntime.Cache.Insert(key, cacheItem, null, expirationUtcTimestamp, Cache.NoSlidingExpiration);
		}

		public void Remove(string key)
		{
			key.ThrowIfNull("key");

			HttpRuntime.Cache.Remove(key);
		}

		public CacheItem Get(string key)
		{
			key.ThrowIfNull("key");

			return (CacheItem)HttpRuntime.Cache.Get(key);
		}
	}
}