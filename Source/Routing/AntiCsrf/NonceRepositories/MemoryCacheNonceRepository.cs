using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

using Junior.Common;

namespace Junior.Route.Routing.AntiCsrf.NonceRepositories
{
	public class MemoryCacheNonceRepository : IAntiCsrfNonceRepository
	{
		private readonly MemoryCache _nonceCache;

		public MemoryCacheNonceRepository(IAntiCsrfConfiguration configuration)
		{
			configuration.ThrowIfNull("configuration");

			_nonceCache = new MemoryCache(configuration.MemoryCacheName);
		}

		public Task AddAsync(Guid sessionId, Guid nonce, DateTime createdUtcTimestamp, DateTime expiresUtcTimestamp)
		{
			if (createdUtcTimestamp.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Created timestamp must be UTC.", "createdUtcTimestamp");
			}
			if (expiresUtcTimestamp.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Expiration timestamp must be UTC.", "expiresUtcTimestamp");
			}

			string cacheKey = GetCacheKey(sessionId, nonce);

			_nonceCache.Set(new CacheItem(cacheKey, expiresUtcTimestamp), new CacheItemPolicy { AbsoluteExpiration = expiresUtcTimestamp });

			return Task.Factory.Empty();
		}

		public Task<bool> ExistsAsync(Guid sessionId, Guid nonce, DateTime currentUtcTimestamp)
		{
			if (currentUtcTimestamp.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Current timestamp must be UTC.", "currentUtcTimestamp");
			}

			string cacheKey = GetCacheKey(sessionId, nonce);
			var expiresUtcTimestamp = (DateTime?)_nonceCache.Get(cacheKey);

			return (expiresUtcTimestamp > currentUtcTimestamp).AsCompletedTask();
		}

		private static string GetCacheKey(Guid sessionId, Guid nonce)
		{
			return String.Format("{0},{1}", sessionId.ToString("N"), nonce.ToString("N"));
		}
	}
}