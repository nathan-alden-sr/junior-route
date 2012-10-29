using System.Web;

using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.CachedResponseHandlers
{
	public interface ICachedResponseHandler
	{
		CachedResponseHandlerResult HandleResponse(HttpRequestBase httpRequest, HttpResponseBase httpResponse, IResponse response, ICache cache, string cacheKey);
	}
}