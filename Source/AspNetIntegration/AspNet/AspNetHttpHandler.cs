using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.ErrorHandlers;
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
		private readonly IEnumerable<IErrorHandler> _errorHandlers;
		private readonly IResponseGenerator[] _responseGenerators;
		private readonly IResponseHandler[] _responseHandlers;
		private readonly IRouteCollection _routes;

		public AspNetHttpHandler(IRouteCollection routes, ICache cache, IEnumerable<IResponseGenerator> responseGenerators, IEnumerable<IResponseHandler> responseHandlers, IEnumerable<IErrorHandler> errorHandlers)
		{
			routes.ThrowIfNull("routes");
			cache.ThrowIfNull("cache");
			responseGenerators.ThrowIfNull("responseGenerators");
			responseHandlers.ThrowIfNull("responseHandlers");
			errorHandlers.ThrowIfNull("errorHandlers");

			_routes = routes;
			_cache = cache;
			_errorHandlers = errorHandlers;
			_responseGenerators = responseGenerators.ToArray();
			_responseHandlers = responseHandlers.ToArray();
		}

		public AspNetHttpHandler(
			IRouteCollection routes,
			ICache cache,
			IEnumerable<IResponseGenerator> responseGenerators,
			IEnumerable<IResponseHandler> responseHandlers,
			IEnumerable<IErrorHandler> errorHandlers,
			IAntiCsrfCookieManager antiCsrfCookieManager,
			IAntiCsrfNonceValidator antiCsrfNonceValidator,
			IAntiCsrfResponseGenerator antiCsrfResponseGenerator)
		{
			routes.ThrowIfNull("routes");
			cache.ThrowIfNull("cache");
			responseGenerators.ThrowIfNull("responseGenerators");
			responseHandlers.ThrowIfNull("responseHandlers");
			errorHandlers.ThrowIfNull("errorHandlers");
			antiCsrfCookieManager.ThrowIfNull("antiCsrfSessionManager");
			antiCsrfNonceValidator.ThrowIfNull("antiCsrfTokenValidator");
			antiCsrfResponseGenerator.ThrowIfNull("antiCsrfResponseGenerator");

			_routes = routes;
			_cache = cache;
			_errorHandlers = errorHandlers;
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

			var contextWrapper = new HttpContextWrapper(context);
			var requestWrapper = new HttpRequestWrapper(context.Request);
			var responseWrapper = new HttpResponseWrapper(context.Response);
			ExceptionDispatchInfo exceptionDispatchInfo = null;

			try
			{
				if (_antiCsrfCookieManager != null && _antiCsrfNonceValidator != null && _antiCsrfResponseGenerator != null)
				{
					if (!String.IsNullOrEmpty(context.Request.ContentType))
					{
						try
						{
							var contentType = new ContentType(context.Request.ContentType);

							if (String.Equals(contentType.MediaType, "application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) || String.Equals(contentType.MediaType, "multipart/form-data", StringComparison.OrdinalIgnoreCase))
							{
								ValidationResult validationResult = await _antiCsrfNonceValidator.ValidateAsync(requestWrapper);
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

					await _antiCsrfCookieManager.ConfigureCookieAsync(requestWrapper, responseWrapper);
				}
				{
					IEnumerable<RouteMatchResult> routeMatchResults = _routes.Select(arg => new RouteMatchResult(arg, arg.MatchesRequest(requestWrapper)));
					IEnumerable<Task<ResponseGenerators.ResponseResult>> responseResultTasks = _responseGenerators.Select(arg => arg.GetResponseAsync(contextWrapper, routeMatchResults));

					foreach (Task<ResponseGenerators.ResponseResult> responseResultTask in responseResultTasks)
					{
						ResponseGenerators.ResponseResult responseResult = await responseResultTask;

						if (responseResult.ResultType != ResponseGenerators.ResponseResultType.ResponseGenerated)
						{
							continue;
						}

						await ProcessResponseAsync(context, await responseResult.Response, responseResult.CacheKey);
						return;
					}
				}
			}
			catch (Exception exception)
			{
				exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
			}

			if (exceptionDispatchInfo != null)
			{
				foreach (IErrorHandler errorHandler in _errorHandlers)
				{
					if ((await errorHandler.HandleAsync(contextWrapper)).ResultType != HandleResultType.Handled)
					{
						continue;
					}

					exceptionDispatchInfo = null;
					break;
				}
			}
			if (exceptionDispatchInfo != null)
			{
				exceptionDispatchInfo.Throw();
			}
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