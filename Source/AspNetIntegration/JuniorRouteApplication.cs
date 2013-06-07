using System;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.ErrorHandlers;
using Junior.Route.AspNetIntegration.RequestFilters;

namespace Junior.Route.AspNetIntegration
{
	public static class JuniorRouteApplication
	{
		private static JuniorRouteApplicationConfiguration _configuration;

		public static void RegisterConfiguration<T>()
			where T : JuniorRouteApplicationConfiguration, new()
		{
			_configuration = new T();
		}

		public static void RegisterConfiguration(JuniorRouteApplicationConfiguration configuration)
		{
			configuration.ThrowIfNull("configuration");

			_configuration = configuration;
		}

		public static void AttachToHttpApplication(HttpApplication application)
		{
			application.ThrowIfNull("application");

			if (_configuration == null)
			{
				throw new InvalidOperationException("No configuration was provided.");
			}

			application.PostAuthenticateRequest += OnApplicationOnPostAuthenticateRequest;
			application.Error += ApplicationOnError;
		}

		private static async void OnApplicationOnPostAuthenticateRequest(object sender, EventArgs e)
		{
			var context = new HttpContextWrapper(HttpContext.Current);
			IRequestFilter[] requestFilters = _configuration.RequestFilters.ToArray();

			if (!requestFilters.Any())
			{
				context.RemapHandler(_configuration.HttpHandler);
				return;
			}

			foreach (IRequestFilter requestFilter in _configuration.RequestFilters)
			{
				if ((await requestFilter.FilterAsync(context)).ResultType == FilterResultType.UseJuniorRouteHandler)
				{
					context.RemapHandler(_configuration.HttpHandler);
					return;
				}
			}
		}

		private static async void ApplicationOnError(object sender, EventArgs e)
		{
			var application = (HttpApplication)sender;
			var context = new HttpContextWrapper(application.Context);

			foreach (IErrorHandler errorHandler in _configuration.ErrorHandlers)
			{
				if ((await errorHandler.HandleAsync(context)).ResultType == HandleResultType.Handled)
				{
					return;
				}
			}

			application.Response.TrySkipIisCustomErrors = true;
			application.Server.ClearError();
		}
	}
}