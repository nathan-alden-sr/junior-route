using System.Web;

using Junior.Common;

namespace Junior.Route.AspNetIntegration
{
	public class JuniorRouteApplicationConfiguration
	{
		public IHttpHandler Handler
		{
			get;
			private set;
		}

		protected JuniorRouteApplicationConfiguration SetHandler(IHttpHandler handler)
		{
			handler.ThrowIfNull("handler");

			Handler = handler;

			return this;
		}
	}
}