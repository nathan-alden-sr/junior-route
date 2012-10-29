using System.Web;

using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.NonCachedResponseHandlers
{
	public interface INonCachedResponseHandler
	{
		NonCachedResponseHandlerResult HandleResponse(HttpRequestBase httpRequest, HttpResponseBase httpResponse, IResponse response);
	}
}