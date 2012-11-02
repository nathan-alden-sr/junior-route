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
		DateTime? Expires
		{
			get;
		}
		TimeSpan? MaxAge
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