using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

using Junior.Common;
using Junior.Route.Http.RequestHeaders;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseHandlers
{
	public class DescriptiveTextStatusCodeHandler : IResponseHandler
	{
		private readonly IEnumerable<StatusAndSubStatusCode> _statusCodes;

		public DescriptiveTextStatusCodeHandler(IEnumerable<StatusAndSubStatusCode> handlesStatusCodes)
		{
			handlesStatusCodes.ThrowIfNull("handlesStatusCodes");

			_statusCodes = handlesStatusCodes;
		}

		public DescriptiveTextStatusCodeHandler(params StatusAndSubStatusCode[] handlesStatusCodes)
			: this((IEnumerable<StatusAndSubStatusCode>)handlesStatusCodes)
		{
		}

		public DescriptiveTextStatusCodeHandler(IEnumerable<int> handlesStatusCodes)
			: this(handlesStatusCodes.Select(arg => new StatusAndSubStatusCode(arg)))
		{
		}

		public DescriptiveTextStatusCodeHandler(params int[] handlesStatusCodes)
			: this((IEnumerable<int>)handlesStatusCodes)
		{
		}

		public DescriptiveTextStatusCodeHandler(IEnumerable<HttpStatusCode> handlesStatusCodes)
			: this(handlesStatusCodes.Select(arg => new StatusAndSubStatusCode(arg)))
		{
		}

		public DescriptiveTextStatusCodeHandler(params HttpStatusCode[] handlesStatusCodes)
			: this((IEnumerable<HttpStatusCode>)handlesStatusCodes)
		{
		}

		public ResponseHandlerResult HandleResponse(HttpRequestBase httpRequest, HttpResponseBase httpResponse, IResponse suggestedResponse, ICache cache, string cacheKey)
		{
			httpRequest.ThrowIfNull("httpRequest");
			httpResponse.ThrowIfNull("httpResponse");
			suggestedResponse.ThrowIfNull("suggestedResponse");

			StatusAndSubStatusCode statusCode = suggestedResponse.StatusCode;

			if (!_statusCodes.Contains(statusCode))
			{
				return ResponseHandlerResult.ResponseNotHandled();
			}

			AcceptHeader[] acceptHeaders = AcceptHeader.ParseMany(httpRequest.Headers["Accept"]).ToArray();

			if (acceptHeaders.Any() && !acceptHeaders.Any(arg => arg.MediaTypeMatches("text/plain")))
			{
				return ResponseHandlerResult.ResponseNotHandled();
			}

			Response response = new Response(statusCode)
				.TextPlain()
				.Content(String.Format("{0} {1}", statusCode.StatusDescription, statusCode.StatusDescription.Length > 0 ? String.Format("({0})", statusCode.StatusDescription) : ""));

			response.CachePolicy.NoClientCaching();

			new CacheResponse(response).WriteResponse(httpResponse);

			httpResponse.TrySkipIisCustomErrors = true;

			return ResponseHandlerResult.ResponseWritten();
		}
	}
}