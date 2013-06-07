using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Junior.Route.AspNetIntegration.ResponseGenerators
{
	public interface IResponseGenerator
	{
		Task<ResponseResult> GetResponse(HttpContextBase context, IEnumerable<RouteMatchResult> routeMatchResults);
	}
}