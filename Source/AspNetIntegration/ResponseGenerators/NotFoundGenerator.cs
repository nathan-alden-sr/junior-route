using System.Collections.Generic;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseGenerators
{
	public class NotFoundGenerator : IResponseGenerator
	{
		public ResponseResult GetResponse(HttpRequestBase request, IEnumerable<RouteMatchResult> routeMatchResults)
		{
			request.ThrowIfNull("request");
			routeMatchResults.ThrowIfNull("routeMatchResults");

			return ResponseResult.NonCachedResponse(Response.NotFound());
		}
	}
}