using System;
using System.Globalization;
using System.Web;

using Junior.Common;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.NonCachedResponseHandlers
{
	public class DescriptiveHtmlStatusCodeHandler : INonCachedResponseHandler
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
			const string format = @"<!DOCTYPE html>
<html>
	<head>
		<title>{0}</title>
		<style>h1 {{ margin: 0; padding: 0; }}</style>
	</head>
	<body>
		<h1>{0}</h1>
		<hr/>
		HTTP {1}{2}
	</body>
</html>";
			Response routeResponse = new Response(statusCode, subStatusCode)
				.TextHtml()
				.Content(String.Format(format, HttpWorkerRequest.GetStatusDescription(statusCode), statusCode, subStatusCode == 0 ? "" : subStatusCode.ToString(CultureInfo.InvariantCulture)));

			new CacheResponse(routeResponse).WriteResponse(httpResponse);

			httpResponse.TrySkipIisCustomErrors = true;

			return NonCachedResponseHandlerResult.ResponseHandled;
		}
	}
}