using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseHandlers
{
	public class NonCacheableResponseHandler : IResponseHandler
	{
		public Task<ResponseHandlerResult> HandleResponse(HttpContextBase context, IResponse suggestedResponse, ICache cache, string cacheKey)
		{
			context.ThrowIfNull("context");
			suggestedResponse.ThrowIfNull("suggestedResponse");

			var cacheResponse = new CacheResponse(suggestedResponse);

			cacheResponse.WriteResponse(context.Response);

			return ResponseHandlerResult.ResponseWritten().AsCompletedTask();
		}
	}
}