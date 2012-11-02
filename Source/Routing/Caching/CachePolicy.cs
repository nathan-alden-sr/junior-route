using System;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Caching
{
	public sealed class CachePolicy : ICachePolicy
	{
		private bool? _allowResponseInBrowserHistory;
		private HttpCacheability? _cacheability;
		private string _eTag;
		private DateTime? _expires;
		private bool _hasPolicy;
		private bool? _ignoreClientCacheControl;
		private TimeSpan? _maxAge;
		private bool? _noStore;
		private bool? _noTransforms;
		private bool? _omitVaryStar;
		private TimeSpan? _proxyMaxAge;
		private HttpCacheRevalidation? _revalidation;
		private bool? _serverCaching;

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
				return _serverCaching == true;
			}
		}

		DateTime? ICachePolicy.Expires
		{
			get
			{
				return _expires;
			}
		}

		TimeSpan? ICachePolicy.MaxAge
		{
			get
			{
				return _maxAge;
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
					policy.SetCacheability(_serverCaching == true ? HttpCacheability.ServerAndNoCache : HttpCacheability.NoCache);
					break;
				case HttpCacheability.Private:
					policy.SetCacheability(_serverCaching == true ? HttpCacheability.ServerAndPrivate : HttpCacheability.Private);
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
			if (_expires != null)
			{
				policy.SetExpires(_expires.Value);
			}
			if (_maxAge != null)
			{
				policy.SetMaxAge(_maxAge.Value);
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
					_eTag = _eTag,
					_expires = _expires,
					_hasPolicy = _hasPolicy,
					_ignoreClientCacheControl = _ignoreClientCacheControl,
					_maxAge = _maxAge,
					_noStore = _noStore,
					_noTransforms = _noTransforms,
					_omitVaryStar = _omitVaryStar,
					_proxyMaxAge = _proxyMaxAge,
					_revalidation = _revalidation,
					_serverCaching = _serverCaching
				};
		}

		public CachePolicy ServerCaching()
		{
			_serverCaching = true;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy NoServerCaching()
		{
			_serverCaching = false;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy PublicClientCaching(DateTime expires)
		{
			if (expires.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Expiration must be UTC.", "expires");
			}

			_cacheability = HttpCacheability.Public;
			_expires = expires;
			_maxAge = null;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy PublicClientCaching(TimeSpan maxAge)
		{
			_cacheability = HttpCacheability.Public;
			_expires = null;
			_maxAge = maxAge;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy PrivateClientCaching(DateTime expires)
		{
			if (expires.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Expiration must be UTC.", "expires");
			}

			_cacheability = HttpCacheability.Private;
			_expires = expires;
			_maxAge = null;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy PrivateClientCaching(TimeSpan maxAge)
		{
			_cacheability = HttpCacheability.Private;
			_expires = null;
			_maxAge = maxAge;
			_hasPolicy = true;

			return this;
		}

		public CachePolicy NoClientCaching()
		{
			_cacheability = HttpCacheability.NoCache;
			_expires = null;
			_maxAge = null;
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
			_expires = null;
			_hasPolicy = false;
			_ignoreClientCacheControl = null;
			_maxAge = null;
			_noStore = null;
			_noTransforms = null;
			_omitVaryStar = null;
			_proxyMaxAge = null;
			_revalidation = null;
			_serverCaching = null;

			return this;
		}
	}
}