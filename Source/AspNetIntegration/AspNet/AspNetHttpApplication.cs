using System.Collections.Generic;
using System.Web;
using System.Web.Routing;

using Junior.Common;

using NathanAlden.JuniorRouting.Core;

namespace NathanAlden.JuniorRouting.AspNetIntegration.AspNet
{
	public class AspNetHttpApplication : HttpApplication, IHttpApplication
	{
		public AspNetHttpApplication(IEnumerable<HttpRoute> routes)
		{
			routes.ThrowIfNull("routes");

			var catchAllRoute = new Route("{*url}", new AspNetRouteHandler(routes));

			RouteTable.Routes.Add(catchAllRoute);
		}
	}
}