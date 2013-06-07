using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.ResponseGenerators;
using Junior.Route.AspNetIntegration.ResponseHandlers;
using Junior.Route.Routing;
using Junior.Route.Routing.AntiCsrf.CookieManagers;
using Junior.Route.Routing.AntiCsrf.NonceValidators;
using Junior.Route.Routing.AntiCsrf.ResponseGenerators;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.Responses;

using ResponseResult = Junior.Route.Routing.AntiCsrf.ResponseGenerators.ResponseResult;
using ResponseResultType = Junior.Route.Routing.AntiCsrf.ResponseGenerators.ResponseResultType;

namespace Junior.Route.AspNetIntegration.AspNet
{
	public class AspNetHttpHandler : HttpTaskAsyncHandler
	{
		private readonly IAntiCsrfCookieManager _antiCsrfCookieManager;
		private readonly IAntiCsrfNonceValidator _antiCsrfNonceValidator;
		private readonly IAntiCsrfResponseGenerator _antiCsrfResponseGenerator;
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
			_cache = cache;
			_responseGenerators = responseGenerators.ToArray();
			_responseHandlers = responseHandlers.ToArray();
		}

		public AspNetHttpHandler(
			IRouteCollection routes,
			ICache cache,
			IEnumerable<IResponseGenerator> responseGenerators,
			IEnumerable<IResponseHandler> responseHandlers,
			IAntiCsrfCookieManager antiCsrfCookieManager,
			IAntiCsrfNonceValidator antiCsrfNonceValidator,
			IAntiCsrfResponseGenerator antiCsrfResponseGenerator)
		{
			routes.ThrowIfNull("routes");
			cache.ThrowIfNull("cache");
			responseGenerators.ThrowIfNull("responseGenerators");
			responseHandlers.ThrowIfNull("responseHandlers");
			antiCsrfCookieManager.ThrowIfNull("antiCsrfSessionManager");
			antiCsrfNonceValidator.ThrowIfNull("antiCsrfTokenValidator");
			antiCsrfResponseGenerator.ThrowIfNull("antiCsrfResponseGenerator");

			_routes = routes;
			_cache = cache;
			_responseGenerators = responseGenerators.ToArray();
			_responseHandlers = responseHandlers.ToArray();
			_antiCsrfCookieManager = antiCsrfCookieManager;
			_antiCsrfNonceValidator = antiCsrfNonceValidator;
			_antiCsrfResponseGenerator = antiCsrfResponseGenerator;
		}

		public override bool IsReusable
		{
			get
			{
				return true;
			}
		}

		public override async Task ProcessRequestAsync(HttpContext context)
		{
			context.ThrowIfNull("context");

			var request = new HttpRequestWrapper(context.Request);
			var response = new HttpResponseWrapper(context.Response);

			if (_antiCsrfCookieManager != null && _antiCsrfNonceValidator != null && _antiCsrfResponseGenerator != null)
			{
				if (!String.IsNullOrEmpty(context.Request.ContentType))
				{
					try
					{
						var contentType = new ContentType(context.Request.ContentType);

						if (String.Equals(contentType.MediaType, "application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) || String.Equals(contentType.MediaType, "multipart/form-data", StringComparison.OrdinalIgnoreCase))
						{
							ValidationResult validationResult = await _antiCsrfNonceValidator.ValidateAsync(request);
							ResponseResult responseResult = await _antiCsrfResponseGenerator.GetResponseAsync(validationResult);

							if (responseResult.ResultType == ResponseResultType.ResponseGenerated)
							{
								await ProcessResponseAsync(context, responseResult.Response, null);
								return;
							}
						}
					}
					catch (FormatException)
					{
					}
				}

				await _antiCsrfCookieManager.ConfigureCookieAsync(request, response);
			}
			{
				IEnumerable<RouteMatchResult> routeMatchResults = await GetRouteMatchResultsAsync(request);
				IEnumerable<Task<ResponseGenerators.ResponseResult>> responseResultTasks = _responseGenerators.Select(arg => arg.GetResponseAsync(new HttpContextWrapper(context), routeMatchResults));

				foreach (Task<ResponseGenerators.ResponseResult> responseResultTask in responseResultTasks)
				{
					ResponseGenerators.ResponseResult responseResult = await responseResultTask;

					if (responseResult.ResultType == ResponseGenerators.ResponseResultType.ResponseGenerated)
					{
						await ProcessResponseAsync(context, await responseResult.Response, responseResult.CacheKey);
						return;
					}
				}
			}
		}

		private async Task<IEnumerable<RouteMatchResult>> GetRouteMatchResultsAsync(HttpRequestBase request)
		{
			var routeMatchResults = new List<RouteMatchResult>();

			foreach (Routing.Route route in _routes)
			{
				MatchResult result = await route.MatchesRequestAsync(request);

				if (result.ResultType == MatchResultType.RouteMatched)
				{
					routeMatchResults.Add(new RouteMatchResult(route, result));
				}
			}

			return routeMatchResults;
		}

		private async Task ProcessResponseAsync(HttpContext context, IResponse response, string cacheKey)
		{
			foreach (IResponseHandler handler in _responseHandlers)
			{
				ResponseHandlerResult handlerResult = await handler.HandleResponseAsync(new HttpContextWrapper(context), response, _cache, cacheKey);

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