using System.Collections.Generic;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.ErrorHandlers;
using Junior.Route.AspNetIntegration.RequestFilters;

namespace Junior.Route.AspNetIntegration
{
	public class JuniorRouteApplicationConfiguration
	{
		private IErrorHandler[] _errorHandlers = new IErrorHandler[0];
		private IRequestFilter[] _requestFilters = new IRequestFilter[0];

		public IHttpHandler HttpHandler
		{
			get;
			private set;
		}

		public IRequestFilter[] RequestFilters
		{
			get
			{
				return _requestFilters;
			}
		}

		public IErrorHandler[] ErrorHandlers
		{
			get
			{
				return _errorHandlers;
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

		protected JuniorRouteApplicationConfiguration SetErrorHandlers(IEnumerable<IErrorHandler> handlers)
		{
			handlers.ThrowIfNull("handlers");

			_errorHandlers = handlers.ToArray();

			return this;
		}

		protected JuniorRouteApplicationConfiguration SetErrorHandlers(params IErrorHandler[] handlers)
		{
			return SetErrorHandlers((IEnumerable<IErrorHandler>)handlers);
		}
	}
}