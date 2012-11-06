using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.ResponseGenerators;
using Junior.Route.AspNetIntegration.ResponseHandlers;
using Junior.Route.Routing;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.AspNet
{
	public class AspNetHttpHandler : IHttpHandler
	{
		private readonly ICache _cache;
		private readonly IResponseGenerator[] _responseGenerators;
		private readonly IResponseHandler[] _responseHandlers;
		private readonly IRouteCollection _routes;

		public AspNetHttpHandler(IRouteCollection routes, ICache cache, IEnumerable<IResponseGenerator> responseGenerators, IEnumerable<IResponseHandler> responseHandlers)
		{
			routes.ThrowIfNull("routes");
			cache.ThrowIfNull("cache");
			responseGenerators.ThrowIfNull("responseGenerators");
			responseHandlers.ThrowIfNull("responseHandlers");

			_routes = routes;
			_responseGenerators = responseGenerators.ToArray();
			_responseHandlers = responseHandlers.ToArray();
			_cache = cache;
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.ThrowIfNull("context");

			var request = new HttpRequestWrapper(context.Request);
			var response = new HttpResponseWrapper(context.Response);
			// ReSharper disable ImplicitlyCapturedClosure
			RouteMatchResult[] routeMatchResults = _routes.Select(arg => new RouteMatchResult(arg, arg.MatchesRequest(request))).ToArray();
			// ReSharper restore ImplicitlyCapturedClosure
			ResponseResult responseResult = _responseGenerators
				.Select(arg => arg.GetResponse(request, routeMatchResults))
				.FirstOrDefault(arg => arg.ResultType != ResponseResultType.ResponseNotGenerated);

			if (responseResult == null)
			{
				throw new ApplicationException("No response was generated.");
			}

			ProcessResponse(request, response, responseResult.Response, responseResult.CacheKey);
		}

		private void ProcessResponse(HttpRequestBase httpRequest, HttpResponseBase httpResponse, IResponse response, string cacheKey)
		{
			foreach (IResponseHandler handler in _responseHandlers)
			{
				ResponseHandlerResult handlerResult = handler.HandleResponse(httpRequest, httpResponse, response, _cache, cacheKey);

				switch (handlerResult.ResultType)
				{
					case ResponseHandlerResultType.ResponseWritten:
						return;
					case ResponseHandlerResultType.ResponseSuggested:
						response = handlerResult.SuggestedResponse;
						break;
				}
			}

			throw new ApplicationException("No response handler handled the response.");
		}
	}
}