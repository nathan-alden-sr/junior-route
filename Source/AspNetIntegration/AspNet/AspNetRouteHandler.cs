using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;

using Junior.Common;

using NathanAlden.JuniorRouting.Core;

namespace NathanAlden.JuniorRouting.AspNetIntegration.AspNet
{
	public class AspNetRouteHandler : IHttpHandler, IRouteHandler
	{
		private readonly IEnumerable<HttpRoute> _routes;

		public AspNetRouteHandler(IEnumerable<HttpRoute> routes)
		{
			routes.ThrowIfNull("routes");

			_routes = routes;
		}

		public void ProcessRequest(HttpContext context)
		{
			var request = new HttpRequestWrapper(context.Request);
			HttpRoute matchingRoute = _routes.FirstOrDefault(arg => arg.MatchesRequest(request));

			if (matchingRoute != null)
			{
				var response = new HttpResponseWrapper(context.Response);

				matchingRoute.GetResponse().WriteResponse(response);
			}
			else
			{
				context.Response.TrySkipIisCustomErrors = true;
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return this;
		}
	}
}