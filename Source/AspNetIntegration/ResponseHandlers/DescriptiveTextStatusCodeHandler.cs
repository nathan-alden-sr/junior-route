using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.Http.RequestHeaders;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.ResponseHandlers
{
	public class DescriptiveTextStatusCodeHandler : IResponseHandler
	{
		private readonly StatusAndSubStatusCode[] _statusCodes;

		public DescriptiveTextStatusCodeHandler(IEnumerable<StatusAndSubStatusCode> handlesStatusCodes)
		{
			handlesStatusCodes.ThrowIfNull("handlesStatusCodes");

			_statusCodes = handlesStatusCodes.ToArray();
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

		public Task<ResponseHandlerResult> HandleResponse(HttpContextBase context, IResponse suggestedResponse, ICache cache, string cacheKey)
		{
			context.ThrowIfNull("context");
			suggestedResponse.ThrowIfNull("suggestedResponse");

			StatusAndSubStatusCode statusCode = suggestedResponse.StatusCode;

			if (!_statusCodes.Contains(statusCode))
			{
				return ResponseHandlerResult.ResponseNotHandled().AsCompletedTask();
			}

			AcceptHeader[] acceptHeaders = AcceptHeader.ParseMany(context.Request.Headers["Accept"]).ToArray();

			if (acceptHeaders.Any() && !acceptHeaders.Any(arg => arg.MediaTypeMatches("text/plain")))
			{
				return ResponseHandlerResult.ResponseNotHandled().AsCompletedTask();
			}

			string content = String.Format(
				"HTTP {0}{1} {2}",
				statusCode.StatusCode,
				statusCode.SubStatusCode == 0 ? "" : "." + statusCode.SubStatusCode.ToString(CultureInfo.InvariantCulture),
				statusCode.StatusDescription.Length > 0 ? String.Format("({0})", statusCode.StatusDescription) : "");
			Response response = new Response(statusCode)
				.TextPlain()
				.Content(content);

			response.CachePolicy.NoClientCaching();

			new CacheResponse(response).WriteResponse(context.Response);

			context.Response.TrySkipIisCustomErrors = true;

			return ResponseHandlerResult.ResponseWritten().AsCompletedTask();
		}
	}
}