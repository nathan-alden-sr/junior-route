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

		public void Initialize(HttpApplication application)
		{
			application.ThrowIfNull("application");

			application.MapRequestHandler += (sender, args) => HttpContext.Current.RemapHandler(Handler);
		}

		protected JuniorRouteApplicationConfiguration SetHandler(IHttpHandler handler)
		{
			handler.ThrowIfNull("handler");

			Handler = handler;

			return this;
		}
	}
}