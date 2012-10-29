using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.CachedResponseHandlers;
using Junior.Route.AspNetIntegration.NonCachedResponseHandlers;
using Junior.Route.AspNetIntegration.ResponseGenerators;
using Junior.Route.Routing;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.AspNet
{
	public class AspNetHttpHandler : IHttpHandler
	{
		private readonly ICache _cache;
		private readonly IEnumerable<ICachedResponseHandler> _cachedResponseHandlers;
		private readonly IEnumerable<INonCachedResponseHandler> _nonCachedResponseHandlers;
		private readonly IEnumerable<IResponseGenerator> _responseGenerators;
		private readonly IRouteCollection _routes;

		public AspNetHttpHandler(
			IRouteCollection routes,
			ICache cache,
			IEnumerable<IResponseGenerator> responseGenerators,
			IEnumerable<ICachedResponseHandler> cachedResponseHandlers,
			IEnumerable<INonCachedResponseHandler> nonCachedResponseHandlers)
		{
			routes.ThrowIfNull("routes");
			cache.ThrowIfNull("cache");
			responseGenerators.ThrowIfNull("responseGenerators");
			cachedResponseHandlers.ThrowIfNull("cachedResponseHandlers");
			nonCachedResponseHandlers.ThrowIfNull("nonCachedResponseHandlers");

			_routes = routes;
			_responseGenerators = responseGenerators;
			_cache = cache;
			_cachedResponseHandlers = cachedResponseHandlers;
			_nonCachedResponseHandlers = nonCachedResponseHandlers;
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
			IEnumerable<RouteMatchResult> routeMatchResults = _routes.Select(arg => new RouteMatchResult(arg, arg.MatchesRequest(request)));
			// ReSharper restore ImplicitlyCapturedClosure
			ResponseResult responseResult = _responseGenerators
				.Select(arg => arg.GetResponse(request, routeMatchResults))
				.FirstOrDefault(arg => arg.ResultType != ResponseResultType.NoResponse);

			if (responseResult == null)
			{
				throw new ApplicationException("No response was generated.");
			}

			if (responseResult.ResultType == ResponseResultType.CachedResponse)
			{
				ProcessCachedResponse(request, response, responseResult.Response, responseResult.CacheKey);
			}
			else
			{
				ProcessNonCachedResponse(request, response, responseResult.Response);
			}
		}

		private void ProcessCachedResponse(HttpRequestBase httpRequest, HttpResponseBase httpResponse, IResponse response, string cacheKey)
		{
			IEnumerable<CachedResponseHandlerResult> results = _cachedResponseHandlers.Select(arg => arg.HandleResponse(httpRequest, httpResponse, response, _cache, cacheKey));

			if (results.Any(arg => arg.ResultType != CachedResponseHandlerResultType.ResponseNotHandled))
			{
				return;
			}

			throw new ApplicationException("No cached response handler found.");
		}

		private void ProcessNonCachedResponse(HttpRequestBase httpRequest, HttpResponseBase httpResponse, IResponse response)
		{
			IEnumerable<NonCachedResponseHandlerResult> results = _nonCachedResponseHandlers.Select(arg => arg.HandleResponse(httpRequest, httpResponse, response));

			if (results.Any(arg => arg != NonCachedResponseHandlerResult.ResponseNotHandled))
			{
				return;
			}

			throw new ApplicationException("No non-cached response handler found.");
		}
	}
}