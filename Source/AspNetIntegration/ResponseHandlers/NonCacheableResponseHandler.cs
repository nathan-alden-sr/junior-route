using System.Web;

using Junior.Common;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseHandlers
{
	public class NonCacheableResponseHandler : IResponseHandler
	{
		public ResponseHandlerResult HandleResponse(HttpRequestBase httpRequest, HttpResponseBase httpResponse, IResponse suggestedResponse, ICache cache, string cacheKey)
		{
			httpRequest.ThrowIfNull("httpRequest");
			httpResponse.ThrowIfNull("httpResponse");
			suggestedResponse.ThrowIfNull("suggestedResponse");

			var cacheResponse = new CacheResponse(suggestedResponse);

			cacheResponse.WriteResponse(httpResponse);

			return ResponseHandlerResult.ResponseWritten();
		}
	}
}