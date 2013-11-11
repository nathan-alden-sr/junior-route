using System;
using System.Web;

namespace Junior.Route.Routing.Caching
{
	public interface ICachePolicy
	{
		bool HasPolicy
		{
			get;
		}

		bool AllowsServerCaching
		{
			get;
		}

		DateTime? ClientCacheExpirationUtcTimestamp
		{
			get;
		}

		TimeSpan? ClientCacheMaxAge
		{
			get;
		}

		DateTime? ServerCacheExpirationUtcTimestamp
		{
			get;
		}

		TimeSpan? ServerCacheMaxAge
		{
			get;
		}

		string ETag
		{
			get;
		}

		void Apply(HttpCachePolicyBase cachePolicy);
		ICachePolicy Clone();
	}
}