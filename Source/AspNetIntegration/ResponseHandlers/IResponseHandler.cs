using System.Threading.Tasks;
using System.Web;

using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseHandlers
{
	public interface IResponseHandler
	{
		Task<ResponseHandlerResult> HandleResponse(HttpContextBase context, IResponse suggestedResponse, ICache cache, string cacheKey);
	}
}