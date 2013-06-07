using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseGenerators
{
	public class NotFoundGenerator : IResponseGenerator
	{
		public Task<ResponseResult> GetResponse(HttpContextBase context, IEnumerable<RouteMatchResult> routeMatchResults)
		{
			context.ThrowIfNull("context");
			routeMatchResults.ThrowIfNull("routeMatchResults");

			return ResponseResult.ResponseGenerated(new Response().NotFound()).AsCompletedTask();
		}
	}
}