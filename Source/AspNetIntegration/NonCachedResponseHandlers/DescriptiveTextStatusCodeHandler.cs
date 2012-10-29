using System;
using System.Globalization;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.NonCachedResponseHandlers
{
	public class DescriptiveTextStatusCodeHandler : INonCachedResponseHandler
	{
		public NonCachedResponseHandlerResult HandleResponse(HttpRequestBase httpRequest, HttpResponseBase httpResponse, IResponse response)
		{
			httpRequest.ThrowIfNull("httpRequest");
			response.ThrowIfNull("httpResponse");

			if (response == null)
			{
				return NonCachedResponseHandlerResult.ResponseNotHandled;
			}

			int statusCode = response.StatusCode;
			int subStatusCode = response.SubStatusCode;
			Response routeResponse = new Response(statusCode, subStatusCode)
				.TextPlain()
				.Content(String.Format("{0} ({1}{2})", HttpWorkerRequest.GetStatusDescription(statusCode), statusCode, subStatusCode == 0 ? "" : subStatusCode.ToString(CultureInfo.InvariantCulture)));

			new CacheResponse(routeResponse).WriteResponse(httpResponse);

			httpResponse.TrySkipIisCustomErrors = true;

			return NonCachedResponseHandlerResult.ResponseHandled;
		}
	}
}