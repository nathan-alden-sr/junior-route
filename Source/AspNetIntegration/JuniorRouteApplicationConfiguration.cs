using System.Collections.Generic;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.ErrorHandlers;

namespace Junior.Route.AspNetIntegration
{
	public class JuniorRouteApplicationConfiguration
	{
		public IHttpHandler HttpHandler
		{
			get;
			private set;
		}

		public IEnumerable<IErrorHandler> ErrorHandlers
		{
			get;
			private set;
		}

		protected JuniorRouteApplicationConfiguration SetHttpHandler(IHttpHandler handler)
		{
			handler.ThrowIfNull("handler");

			HttpHandler = handler;

			return this;
		}

		protected JuniorRouteApplicationConfiguration SetErrorHandlers(IEnumerable<IErrorHandler> handlers)
		{
			handlers.ThrowIfNull("handlers");

			ErrorHandlers = handlers;

			return this;
		}

		protected JuniorRouteApplicationConfiguration SetErrorHandlers(params IErrorHandler[] handlers)
		{
			return SetErrorHandlers((IEnumerable<IErrorHandler>)handlers);
		}
	}
}