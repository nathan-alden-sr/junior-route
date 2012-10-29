using System.Collections.Generic;
using System.Web;

namespace Junior.Route.AspNetIntegration.ResponseGenerators
{
	public interface IResponseGenerator
	{
		ResponseResult GetResponse(HttpRequestBase request, IEnumerable<RouteMatchResult> routeMatchResults);
	}
}