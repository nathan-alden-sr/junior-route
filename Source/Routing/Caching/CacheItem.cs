using System;

using Junior.Common;

namespace Junior.Route.Routing.Caching
{
	public class CacheItem
	{
		private readonly DateTime _cachedUtcTimestamp;
		private readonly DateTime? _expiresUtcTimestamp;
		private readonly CacheResponse _response;

		public CacheItem(CacheResponse response, DateTime cachedUtcTimestamp, DateTime? expiresUtcTimestamp = null)
		{
			response.ThrowIfNull("response");
			if (cachedUtcTimestamp.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Cached timestamp must be UTC.", "cachedUtcTimestamp");
			}
			if (expiresUtcTimestamp != null && expiresUtcTimestamp.Value.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Absolute expiration timestamp must be UTC.", "expiresUtcTimestamp");
			}

			_response = response;
			_cachedUtcTimestamp = cachedUtcTimestamp;
			_expiresUtcTimestamp = expiresUtcTimestamp;
		}

		public CacheResponse Response
		{
			get
			{
				return _response;
			}
		}

		public DateTime CachedUtcTimestamp
		{
			get
			{
				return _cachedUtcTimestamp;
			}
		}

		public DateTime? ExpiresUtcTimestamp
		{
			get
			{
				return _expiresUtcTimestamp;
			}
		}
	}
}