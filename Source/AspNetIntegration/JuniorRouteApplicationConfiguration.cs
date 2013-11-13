using System.Collections.Generic;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.RequestFilters;

namespace Junior.Route.AspNetIntegration
{
	public class JuniorRouteApplicationConfiguration
	{
		private IEnumerable<IRequestFilter> _requestFilters = Enumerable.Empty<IRequestFilter>();

		public IHttpHandler HttpHandler
		{
			get;
			private set;
		}

		public IEnumerable<IRequestFilter> RequestFilters
		{
			get
			{
				return _requestFilters;
			}
		}

		protected JuniorRouteApplicationConfiguration SetHttpHandler(IHttpHandler handler)
		{
			handler.ThrowIfNull("handler");

			HttpHandler = handler;

			return this;
		}

		protected JuniorRouteApplicationConfiguration SetRequestFilters(IEnumerable<IRequestFilter> filters)
		{
			filters.ThrowIfNull("filters");

			_requestFilters = filters.ToArray();

			return this;
		}

		protected JuniorRouteApplicationConfiguration SetRequestFilters(params IRequestFilter[] filters)
		{
			return SetRequestFilters((IEnumerable<IRequestFilter>)filters);
		}
	}
}