using System;
using System.Linq;
using System.Web;

using Junior.Common;
using Junior.Route.AspNetIntegration.ErrorHandlers;

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

			application.PostAuthenticateRequest += (sender, args) => HttpContext.Current.RemapHandler(_configuration.HttpHandler);
			application.Error += ApplicationOnError;
		}

		private static void ApplicationOnError(object sender, EventArgs e)
		{
			if (_configuration.ErrorHandlers == null)
			{
				return;
			}

			var application = (HttpApplication)sender;
			var context = new HttpContextWrapper(application.Context);

			if (_configuration.ErrorHandlers.All(arg => arg.Handle(context).ResultType != HandleResultType.Handled))
			{
				return;
			}

			application.Response.TrySkipIisCustomErrors = true;
			application.Server.ClearError();
		}
	}
}