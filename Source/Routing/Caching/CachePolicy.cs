using System;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Caching
{
	public sealed class CachePolicy : ICachePolicy
	{
		private bool? _allowResponseInBrowserHistory;
		private HttpCacheability? _cacheability;
		private DateTime? _clientCacheExpirationUtcTimestamp;
		private TimeSpan? _clientCacheMaxAge;
		private string _eTag;
		private bool _hasPolicy;
		private bool? _ignoreClientCacheControl;
		private bool? _noStore;
		private bool? _noTransforms;
		private bool? _omitVaryStar;
		private TimeSpan? _proxyMaxAge;
		private HttpCacheRevalidation? _revalidation;
		private DateTime? _serverCacheExpirationUtcTimestamp;
		private TimeSpan? _serverCacheMaxAge;
		private bool? _allowsServerCaching;

		bool ICachePolicy.HasPolicy
		{
			get
			{
				return _hasPolicy;
			}
		}

		bool ICachePolicy.AllowsServerCaching
		{
			get
			{
				return _allowsServerCaching == true;
			}
		}

		DateTime? ICachePolicy.ClientCacheExpirationUtcTimestamp
		{
			get
			{
				return _clientCacheExpirationUtcTimestamp;
			}
		}

		TimeSpan? ICachePolicy.ClientCacheMaxAge
		{
			get
			{
				return _clientCacheMaxAge;
			}
		}

		DateTime? ICachePolicy.ServerCacheExpirationUtcTimestamp
		{
			get
			{
				return _serverCacheExpirationUtcTimestamp;
			}
		}

		TimeSpan? ICachePolicy.ServerCacheMaxAge
		{
			get
			{
				return _serverCacheMaxAge;
			}
		}

		string ICachePolicy.ETag
		{
			get
			{
				return _eTag;
			}
		}

		public void Apply(HttpCachePolicyBase policy)
		{
			policy.ThrowIfNull("policy");

			if (!_hasPolicy)
			{
				return;
			}

			switch (_cacheability)
			{
				case HttpCacheability.NoCache:
					policy.SetCacheability(_allowsServerCaching == true ? HttpCacheability.ServerAndNoCache : HttpCacheability.NoCache);
					break;
				case HttpCacheability.Private:
					policy.SetCacheability(_allowsServerCaching == true ? HttpCacheability.ServerAndPrivate : HttpCacheability.Private);
					break;
				case HttpCacheability.Public:
					policy.SetCacheability(HttpCacheability.Public);
					break;
			}
			if (_noStore == true)
			{
				policy.SetNoStore();
			}
			if (_noTransforms == true)
			{
				policy.SetNoTransforms();
			}
			if (_clientCacheExpirationUtcTimestamp != null)
			{
				policy.SetExpires(_clientCacheExpirationUtcTimestamp.Value);
			}
			if (_clientCacheMaxAge != null)
			{
				policy.SetMaxAge(_clientCacheMaxAge.Value);
			}
			if (_allowResponseInBrowserHistory != null)
			{
				policy.SetAllowResponseInBrowserHistory(_allowResponseInBrowserHistory.Value);
			}
			if (_eTag != null)
			{
				policy.SetETag(_eTag);
			}
			if (_omitVaryStar != null)
			{
				policy.SetOmitVaryStar(_omitVaryStar.Value);
			}
			if (_proxyMaxAge != null)
			{
				policy.SetProxyMaxAge(_proxyMaxAge.Value);
			}
			if (_revalidation != null)
			{
				policy.SetRevalidation(_revalidation.Value);
			}
		}

		ICachePolicy ICachePolicy.Clone()
		{
			return Clone();
		}

		public CachePolicy Clone()
		{
			return new CachePolicy
				{
					_allowResponseInBrowserHistory = _allowResponseInBrowserHistory,
					_cacheability = _cacheability,
					_clientCacheExpirationUtcTimestamp = _clientCacheExpirationUtcTimestamp,
					_clientCacheMaxAge = _clientCacheMaxAge,
					_eTag = _eTag,
					_hasPolicy = _hasPolicy,
					_ignoreClientCacheControl = _ignoreClientCacheControl,
					_noStore = _noStore,
					_noTransforms = _noTransforms,
					_omitVaryStar = _omitVaryStar,
					_proxyMaxAge = _proxyMaxAge,
					_revalidation = _revalidation,
					_serverCacheExpirationUtcTimestamp = _serverCacheExpirationUtcTimestamp,
					_serverCacheMaxAge = _serverCacheMaxAge,
					_allowsServerCaching = _allowsServerCaching
				};
		}

		public CachePolicy PublicClientCaching(DateTime expirationUtcTimestamp)
		{
			if (expirationUtcTimestamp.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Expiration must be UTC.", "expirationUtcTimestamp");
			}

			_cacheability = HttpCacheability.Public;
			_clientCacheExpirationUtcTimestamp = expirationUtcTimestamp;
			_clientCacheMaxAge = null;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy PublicClientCaching(TimeSpan maxAge)
		{
			_cacheability = HttpCacheability.Public;
			_clientCacheExpirationUtcTimestamp = null;
			_clientCacheMaxAge = maxAge;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy PrivateClientCaching(DateTime expirationUtcTimestamp)
		{
			if (expirationUtcTimestamp.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Expiration must be UTC.", "expirationUtcTimestamp");
			}

			_cacheability = HttpCacheability.Private;
			_clientCacheExpirationUtcTimestamp = expirationUtcTimestamp;
			_clientCacheMaxAge = null;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy PrivateClientCaching(TimeSpan maxAge)
		{
			_cacheability = HttpCacheability.Private;
			_clientCacheExpirationUtcTimestamp = null;
			_clientCacheMaxAge = maxAge;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy NoClientCaching()
		{
			_cacheability = HttpCacheability.NoCache;
			_clientCacheExpirationUtcTimestamp = null;
			_clientCacheMaxAge = null;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy ServerCaching(DateTime expirationUtcTimestamp)
		{
			if (expirationUtcTimestamp.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Expiration must be UTC.", "expirationUtcTimestamp");
			}

			_allowsServerCaching = true;
			_serverCacheExpirationUtcTimestamp = expirationUtcTimestamp;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy ServerCaching(TimeSpan maxAge)
		{
			_allowsServerCaching = true;
			_serverCacheMaxAge = maxAge;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy NoServerCaching()
		{
			_allowsServerCaching = false;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy NoStore()
		{
			_noStore = true;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy NoTransforms()
		{
			_noTransforms = true;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy AllowResponseInBrowserHistory(bool allow)
		{
			_allowResponseInBrowserHistory = allow;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy ETag(string eTag)
		{
			eTag.ThrowIfNull("eTag");

			_eTag = eTag;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy OmitVaryStar(bool omit)
		{
			_omitVaryStar = omit;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy ProxyMaxAge(TimeSpan maxAge)
		{
			_proxyMaxAge = maxAge;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy Revalidation(HttpCacheRevalidation revalidation)
		{
			_revalidation = revalidation;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy Reset()
		{
			_allowResponseInBrowserHistory = null;
			_cacheability = null;
			_eTag = null;
			_clientCacheExpirationUtcTimestamp = null;
			_hasPolicy = false;
			_ignoreClientCacheControl = null;
			_clientCacheMaxAge = null;
			_noStore = null;
			_noTransforms = null;
			_omitVaryStar = null;
			_proxyMaxAge = null;
			_revalidation = null;
			_allowsServerCaching = null;

			return this;
		}
	}
}