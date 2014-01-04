using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.ErrorHandlers;
using Junior.Route.AspNetIntegration.ResponseGenerators;
using Junior.Route.AspNetIntegration.ResponseHandlers;
using Junior.Route.Routing;
using Junior.Route.Routing.Caching;
using Junior.Route.Routing.RequestValidators;
using Junior.Route.Routing.Responses;

namespace Junior.Route.AspNetIntegration.AspNet
{
	public class AspNetHttpHandler : HttpTaskAsyncHandler
	{
		private readonly ICache _cache;
		private readonly IEnumerable<IErrorHandler> _errorHandlers;
		private readonly IEnumerable<IRequestValidator> _requestValidators;
		private readonly IResponseGenerator[] _responseGenerators;
		private readonly IResponseHandler[] _responseHandlers;
		private readonly IRouteCollection _routes;

		public AspNetHttpHandler(
			IRouteCollection routes,
			ICache cache,
			IEnumerable<IRequestValidator> requestValidators,
			IEnumerable<IResponseGenerator> responseGenerators,
			IEnumerable<IResponseHandler> responseHandlers,
			IEnumerable<IErrorHandler> errorHandlers)
		{
			routes.ThrowIfNull("routes");
			cache.ThrowIfNull("cache");
			requestValidators.ThrowIfNull("requestValidators");
			responseGenerators.ThrowIfNull("responseGenerators");
			responseHandlers.ThrowIfNull("responseHandlers");
			errorHandlers.ThrowIfNull("errorHandlers");

			_routes = routes;
			_cache = cache;
			_requestValidators = requestValidators;
			_errorHandlers = errorHandlers;
			_responseGenerators = responseGenerators.ToArray();
			_responseHandlers = responseHandlers.ToArray();
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
				foreach (IRequestValidator requestValidator in _requestValidators)
				{
					ValidateResult validateResult = await requestValidator.Validate(requestWrapper, responseWrapper);

					if (validateResult.ResultType == ValidateResultType.RequestValidated)
					{
						continue;
					}

					await ProcessResponseAsync(context, validateResult.Response, null);
					return;
				}

				IEnumerable<RouteMatchResult> routeMatchResults = _routes.Select(arg => new RouteMatchResult(arg, arg.MatchesRequest(requestWrapper)));
				IEnumerable<Task<ResponseResult>> responseResultTasks = _responseGenerators.Select(arg => arg.GetResponseAsync(contextWrapper, routeMatchResults));

				foreach (Task<ResponseResult> responseResultTask in responseResultTasks)
				{
					ResponseResult responseResult = await responseResultTask;

					if (responseResult.ResultType != ResponseResultType.ResponseGenerated)
					{
						continue;
					}

					await ProcessResponseAsync(context, await responseResult.Response, responseResult.CacheKey);
					return;
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
					if ((await errorHandler.HandleAsync(contextWrapper, exceptionDispatchInfo.SourceException)).ResultType != HandleResultType.Handled)
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